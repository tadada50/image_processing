// Image prep math (pure, works in browser + node). Values are luminance 0..1 (1 = white = no string).
(function(root){
function toGray(rgba,n){const g=new Float32Array(n);for(let i=0;i<n;i++)g[i]=(0.299*rgba[i*4]+0.587*rgba[i*4+1]+0.114*rgba[i*4+2])/255;return g;}
// Contrast-limited histogram equalization over pixels where mask=1. clip: multiple of mean bin height (e.g. 3).
function equalize(g,mask,clip){const B=256,h=new Float64Array(B);let n=0;for(let i=0;i<g.length;i++)if(mask[i]){h[Math.min(255,g[i]*255|0)]++;n++;}
 if(!n)return g;const lim=clip*n/B;let ex=0;for(let b=0;b<B;b++)if(h[b]>lim){ex+=h[b]-lim;h[b]=lim;}for(let b=0;b<B;b++)h[b]+=ex/B;
 const cdf=new Float64Array(B);let s=0;for(let b=0;b<B;b++){s+=h[b];cdf[b]=s/n;}
 const o=new Float32Array(g.length);for(let i=0;i<g.length;i++)o[i]=mask[i]?cdf[Math.min(255,g[i]*255|0)]:g[i];return o;}
// Unsharp mask with 3x3 box blur. amount 0..3
function sharpen(g,S,amount){if(!amount)return g;const o=new Float32Array(g.length);
 for(let y=0;y<S;y++)for(let x=0;x<S;x++){let s=0,c=0;for(let dy=-1;dy<=1;dy++)for(let dx=-1;dx<=1;dx++){const X=x+dx,Y=y+dy;if(X>=0&&Y>=0&&X<S&&Y<S){s+=g[Y*S+X];c++;}}
  const v=g[y*S+x];o[y*S+x]=Math.max(0,Math.min(1,v+amount*(v-s/c)));}return o;}
// p: {brightness:-1..1, contrast:-1..1, gamma:0.2..5, equalize:bool, clip, sharpen:0..3, invert:bool}
function process(rgba,S,p){const n=S*S,c=(S-1)/2,mask=new Uint8Array(n);for(let i=0;i<n;i++)mask[i]=Math.hypot(i%S-c,(i/S|0)-c)<=c?1:0;
 let g=toGray(rgba,n);if(p.equalize)g=equalize(g,mask,p.clip||3);g=sharpen(g,S,p.sharpen||0);
 const k=Math.tan((Math.max(-0.99,Math.min(0.99,p.contrast||0))+1)*Math.PI/4),gm=p.gamma||1;
 for(let i=0;i<n;i++){if(!mask[i]){g[i]=1;continue;}let v=(g[i]-0.5)*k+0.5+(p.brightness||0);v=Math.max(0,Math.min(1,v));v=Math.pow(v,gm);if(p.invert)v=1-v;g[i]=v;}
 return g;}
root.Prep={toGray,equalize,sharpen,process};
})(typeof self!=='undefined'?self:globalThis);
