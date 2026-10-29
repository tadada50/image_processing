// before/after on real portrait. node compare.js
const fs=require('fs'),jpeg=require('jpeg-js'),{PNG}=require('pngjs');const S=500,N=200;
const j=jpeg.decode(fs.readFileSync(__dirname+'/p.jpg'),{useTArray:true}),m=Math.min(j.width,j.height);
// crop: centered horizontally, top-biased for portrait
const ox=(j.width-m)/2|0,oy=Math.max(0,((j.height-m)*0.25)|0),rgba=new Uint8ClampedArray(S*S*4);
for(let y=0;y<S;y++)for(let x=0;x<S;x++){const sx=ox+(x*m/S|0),sy=oy+(y*m/S|0),s=(sy*j.width+sx)*4,d=(y*S+x)*4;for(let c=0;c<4;c++)rgba[d+c]=j.data[s+c];}
require('/workspace/linify-pages/prep.js');
const c0=(S-1)/2,inside=i=>Math.hypot(i%S-c0,(i/S|0)-c0)<=c0;
function out(name,steps,w){ // render with alpha compositing, AA-free but width/alpha physical
 const R=S,img=new Float32Array(R*R);const a=Math.min(1,w);const P=globalThis.Linify.pinsFor(N,R);
 for(const[x,y]of steps){const p=P[x],q=P[y],n=Math.ceil(Math.hypot(q[0]-p[0],q[1]-p[1]))*2;let prev=-1;
  for(let k=0;k<=n;k++){const X=p[0]+(q[0]-p[0])*k/n,Y=p[1]+(q[1]-p[1])*k/n,x0=Math.floor(X),y0=Math.floor(Y),fx=X-x0,fy=Y-y0;
   for(const[cx,cy,ww]of[[x0,y0,(1-fx)*(1-fy)],[x0+1,y0,fx*(1-fy)],[x0,y0+1,(1-fx)*fy],[x0+1,y0+1,fx*fy]]){if(cx<0||cy<0||cx>=R||cy>=R)continue;const i=cy*R+cx;img[i]+=ww*0.5*a*(1-img[i]);}}}
 const png=new PNG({width:R,height:R});for(let i=0;i<R*R;i++){const v=inside(i)?Math.round((1-img[i])*255):255;png.data[i*4]=png.data[i*4+1]=png.data[i*4+2]=v;png.data[i*4+3]=255;}
 fs.writeFileSync(name,PNG.sync.write(png));}
// BEFORE: old engine, user's iPhone params; rendered opaque-ish like old UI (alpha 0.3)
delete globalThis.Linify;require('./engine_old.js');
let g=Prep.process(rgba,S,{});let dark=new Float32Array(S*S);for(let i=0;i<S*S;i++)if(inside(i))dark[i]=1-g[i];
let st=[...globalThis.Linify.run(dark,S,{pins:N,lines:2000,opacity:0.2,minSkip:10})].map(r=>[r.from,r.to]);
out('/workspace/linify-web/sample_portrait_before.png',st,1);console.log('before lines',st.length);
// AFTER
delete globalThis.Linify;require('/workspace/linify-pages/engine.js');
const D=500,thr=0.25,w=thr/D*S; // px width
g=Prep.process(rgba,S,{gamma:1.3,contrast:0.2});const t=new Float32Array(S*S);for(let i=0;i<S*S;i++)if(inside(i))t[i]=1-g[i];
const t0=Date.now();st=[...globalThis.Linify.run(t,S,{pins:N,lines:4000,alpha:w,minSkip:15})].map(r=>[r.from,r.to]);
const len=st.reduce((s,[a,b])=>s+globalThis.Linify.chord(a,b,N,D/1000),0);
out('/workspace/linify-web/sample_portrait.png',st,w);fs.writeFileSync('/workspace/linify-web/sample_portrait_steps.csv','step,from_pin,to_pin\n'+st.map((s,i)=>`${i+1},${s[0]},${s[1]}`).join('\n'));
console.log('after lines',st.length,'alpha',w,'thread m',len.toFixed(0),'ms',Date.now()-t0);
