// Headless test: synthetic image (dark disk + gradient), run engine, render PNG via pure-JS rasterizer.
require('./engine.js');const zlib=require('zlib'),fs=require('fs');const S=400,N=200,c=(S-1)/2;
const dark=new Float32Array(S*S);for(let y=0;y<S;y++)for(let x=0;x<S;x++){const r=Math.hypot(x-c,y-c);if(r>c)continue;
 let v=0.15+0.3*x/S; if(Math.hypot(x-150,y-160)<60||Math.hypot(x-260,y-160)<40)v=0.95; if(Math.abs(y-280)<15&&Math.abs(x-200)<90)v=0.9; dark[y*S+x]=v;}
const steps=[];const g=Linify.run(dark,S,{pins:N,lines:2500,opacity:0.2,minSkip:10});let r;while(!(r=g.next()).done)steps.push([r.value.from,r.value.to]);
const P=Linify.pinsFor(N,S),img=new Float32Array(S*S).fill(1);
for(const [a,b] of steps){const p=P[a],q=P[b],n=Math.ceil(Math.hypot(q[0]-p[0],q[1]-p[1]));for(let k=0;k<=n;k++){const x=Math.round(p[0]+(q[0]-p[0])*k/n),y=Math.round(p[1]+(q[1]-p[1])*k/n);if(x>=0&&y>=0&&x<S&&y<S)img[y*S+x]*=0.75;}}
const raw=Buffer.alloc((S+1)*S);for(let y=0;y<S;y++){raw[y*(S+1)]=0;for(let x=0;x<S;x++)raw[y*(S+1)+1+x]=Math.round(img[y*S+x]*255);}
const crc=b=>{let c,t=[];for(let n=0;n<256;n++){c=n;for(let k=0;k<8;k++)c=c&1?0xedb88320^(c>>>1):c>>>1;t[n]=c>>>0;}c=~0;for(const x of b)c=t[(c^x)&255]^(c>>>8);return (~c)>>>0;};
const chunk=(t,d)=>{const l=Buffer.alloc(4);l.writeUInt32BE(d.length);const td=Buffer.concat([Buffer.from(t),d]);const cr=Buffer.alloc(4);cr.writeUInt32BE(crc(td));return Buffer.concat([l,td,cr]);};
const ih=Buffer.alloc(13);ih.writeUInt32BE(S,0);ih.writeUInt32BE(S,4);ih[8]=8;ih[9]=0;
fs.writeFileSync(__dirname+'/sample.png',Buffer.concat([Buffer.from([137,80,78,71,13,10,26,10]),chunk('IHDR',ih),chunk('IDAT',zlib.deflateSync(raw)),chunk('IEND',Buffer.alloc(0))]));
fs.writeFileSync(__dirname+'/sample_steps.txt',steps.map((s,i)=>`Step ${i+1}: pin ${s[0]} -> pin ${s[1]}`).join('\n'));
console.log('steps',steps.length,'first',steps.slice(0,3));
