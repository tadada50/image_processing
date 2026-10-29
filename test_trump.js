const fs=require('fs'),{PNG}=require('pngjs');require('/workspace/linify-pages/prep.js');require('/workspace/linify-pages/engine.js');
const BG=+(process.argv[3]||255);
const src=PNG.sync.read(fs.readFileSync('t_cut.png')),S=500,N=200,D=500,thr=0.25,w=thr/D*S;
const [cx,cy,side]=[457,450,620].map(Number),x0=cx-side/2,y0=cy-side/2,rgba=new Uint8ClampedArray(S*S*4);
for(let y=0;y<S;y++)for(let x=0;x<S;x++){const sx=Math.round(x0+x*side/S),sy=Math.round(y0+y*side/S),d=(y*S+x)*4;let r=BG,g=BG,b=BG;
 if(sx>=0&&sy>=0&&sx<src.width&&sy<src.height){const s=(sy*src.width+sx)*4,a=src.data[s+3]/255;r=src.data[s]*a+255*(1-a)*BG/255;g=src.data[s+1]*a+BG*(1-a);b=src.data[s+2]*a+BG*(1-a);}
 rgba[d]=r;rgba[d+1]=g;rgba[d+2]=b;rgba[d+3]=255;}
const P0=JSON.parse(process.argv[2]||'{"gamma":1.3,"contrast":0.2}');const g=Prep.process(rgba,S,P0);
const c0=(S-1)/2,inside=i=>Math.hypot(i%S-c0,(i/S|0)-c0)<=c0,t=new Float32Array(S*S);for(let i=0;i<S*S;i++)if(inside(i))t[i]=1-g[i];
const sp=new PNG({width:S,height:S});for(let i=0;i<S*S;i++){sp.data.fill(Math.round(g[i]*255),i*4,i*4+3);sp.data[i*4+3]=255;}fs.writeFileSync('/workspace/linify-web/trump_v2_source.png',PNG.sync.write(sp));
const F=JSON.parse(process.argv[4]||"null"),WM=F?Linify.weightMap(S,F[0],F[1],F[2],F[3]):null;
const st=[...Linify.run(t,S,{pins:N,lines:4000,alpha:w,minSkip:15,weight:WM})].map(r=>[r.from,r.to]);
const len=st.reduce((s,[a,b])=>s+Linify.chord(a,b,N,D/1000),0);
// hi-res render 2000px: thread = 0.25/500*2000 = 1px wide, alpha 1 -> AA coverage accumulate
const R=2000,wr=thr/D*R,img=new Float32Array(R*R),P=Linify.pinsFor(N,R),a=Math.min(1,wr),rc=(R-1)/2;
for(const[i,j]of st){const p=P[i],q=P[j],n=Math.ceil(Math.hypot(q[0]-p[0],q[1]-p[1]))*2;for(let k=0;k<=n;k++){const X=p[0]+(q[0]-p[0])*k/n,Y=p[1]+(q[1]-p[1])*k/n,xa=Math.floor(X),ya=Math.floor(Y),fx=X-xa,fy=Y-ya;
 for(const[ex,ey,ww]of[[xa,ya,(1-fx)*(1-fy)],[xa+1,ya,fx*(1-fy)],[xa,ya+1,(1-fx)*fy],[xa+1,ya+1,fx*fy]]){if(ex<0||ey<0||ex>=R||ey>=R)continue;const q2=ey*R+ex;img[q2]+=ww*0.5*a*(1-img[q2]);}}}
const o=new PNG({width:R,height:R});for(let i=0;i<R*R;i++){const v=Math.hypot(i%R-rc,(i/R|0)-rc)<=rc?Math.round((1-img[i])*255):255;o.data.fill(v,i*4,i*4+3);o.data[i*4+3]=255;}
fs.writeFileSync('/workspace/linify-web/trump_v2.png',PNG.sync.write(o));
const hdr=`Donald Trump string art. Pins numbered 0-${N-1}, pin 0 at top, clockwise. Start at pin 0.\nFrame 50 cm, thread 0.25 mm, ${st.length} lines, estimated thread length ${len.toFixed(1)} m (chords only, add slack for wrapping pins).\n`;
fs.writeFileSync('/workspace/linify-web/trump_v2_steps.txt',hdr+st.map((s,i)=>`Step ${i+1}: pin ${s[0]} -> pin ${s[1]}`).join('\n')+'\n');
fs.writeFileSync('/workspace/linify-web/trump_v2_steps.csv','step,from_pin,to_pin\n'+st.map((s,i)=>`${i+1},${s[0]},${s[1]}`).join('\n')+'\n');
console.log('lines',st.length,'thread m',len.toFixed(1));
