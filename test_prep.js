require('./prep.js');const a=require('assert');const S=50,n=S*S,rgba=new Uint8ClampedArray(n*4);
for(let i=0;i<n;i++){const v=100+(i%S);rgba[i*4]=rgba[i*4+1]=rgba[i*4+2]=v;rgba[i*4+3]=255;}
const base=Prep.process(rgba,S,{});const ctr=1275;a(Math.abs(base[ctr]-(100+25)/255)<1e-6,'identity');a.equal(base[0],1,'outside circle white');
a(Prep.process(rgba,S,{brightness:0.2})[ctr]>base[ctr],'brightness');a(Math.abs(Prep.process(rgba,S,{invert:true})[ctr]-(1-base[ctr]))<1e-6,'invert');
a(Prep.process(rgba,S,{gamma:2})[ctr]<base[ctr],'gamma darkens');
const hi=Prep.process(rgba,S,{contrast:0.5}),l=1251,r=1298;a(hi[r]-hi[l]>base[r]-base[l],'contrast widens');
const eq=Prep.process(rgba,S,{equalize:true,clip:100});let mn=1,mx=0;for(let i=0;i<n;i++)if(base[i]<1){mn=Math.min(mn,eq[i]);mx=Math.max(mx,eq[i]);}a(mx-mn>0.8,'equalize spreads '+mn+' '+mx);
a(Prep.process(rgba,S,{sharpen:1})[ctr]>=0,'sharpen ok');console.log('prep tests passed');
