// Linify engine v2: circular string art, greedy residual method.
// Pins numbered 0..N-1, pin 0 at top (12 o'clock), increasing clockwise.
// target: Float32Array size*size, desired darkness 0 (white) .. 1 (black); outside circle ignored.
// Each line adds coverage c += a*w*(1-c) (a = per-line alpha from physical thread width), matching canvas alpha compositing.
// Score of a line = mean over its pixels of (target - coverage): lines over white/background are penalised.
(function(root){
function pinsFor(n,size){const c=(size-1)/2,r=c,p=[];for(let i=0;i<n;i++){const a=2*Math.PI*i/n;p.push([c+r*Math.sin(a),c-r*Math.cos(a)]);}return p;}
function lineSamples(p,q,size){const dx=q[0]-p[0],dy=q[1]-p[1],len=Math.hypot(dx,dy),n=Math.max(2,Math.ceil(len));
 const m=new Map();for(let k=0;k<=n;k++){const x=p[0]+dx*k/n,y=p[1]+dy*k/n,x0=Math.floor(x),y0=Math.floor(y),fx=x-x0,fy=y-y0;
  const cells=[[x0,y0,(1-fx)*(1-fy)],[x0+1,y0,fx*(1-fy)],[x0,y0+1,(1-fx)*fy],[x0+1,y0+1,fx*fy]];
  for(const [cx,cy,w] of cells){if(w<=0||cx<0||cy<0||cx>=size||cy>=size)continue;const i=cy*size+cx;m.set(i,(m.get(i)||0)+w);}}
 const idx=new Int32Array(m.size),w=new Float32Array(m.size);let j=0,tw=0;for(const [i,v] of m){idx[j]=i;w[j]=Math.min(1,v);tw+=w[j];j++;}return {idx,w,tw};}
// o: {pins, lines, alpha, minSkip, autoStop(bool, default true)}
function* run(target,size,o){const N=o.pins,P=pinsFor(N,size),cache=new Map(),skip=o.minSkip||0,a=Math.min(1,o.alpha),auto=o.autoStop!==false;
 const cov=new Float32Array(size*size);
 const get=(x,y)=>{const k=x<y?x*N+y:y*N+x;let s=cache.get(k);if(!s){s=lineSamples(P[x],P[y],size);cache.set(k,s);}return s;};
 let cur=0,last=-1;const used=new Set();
 for(let s=0;s<o.lines;s++){let best=-1,bs=-1e9;
  for(let j=0;j<N;j++){let d=Math.abs(j-cur);d=Math.min(d,N-d);if(d<=skip||j===last)continue;
   const k=cur<j?cur*N+j:j*N+cur;if(used.has(k))continue;const L=get(cur,j);let t=0;
   for(let i=0;i<L.idx.length;i++){const q=L.idx[i];t+=(target[q]-cov[q])*L.w[i];}const sc=t/L.tw;if(sc>bs){bs=sc;best=j;}}
  if(best<0||(auto&&bs<=0))break;const L=get(cur,best);
  for(let i=0;i<L.idx.length;i++){const q=L.idx[i];cov[q]+=a*L.w[i]*(1-cov[q]);}
  used.add(cur<best?cur*N+best:best*N+cur);last=cur;cur=best;yield {step:s+1,from:last,to:cur,score:bs};}}
// physical chord length for pins a,b on a circle of diameter D
function chord(a,b,N,D){return D*Math.abs(Math.sin(Math.PI*(b-a)/N));}
root.Linify={pinsFor,run,chord};
})(typeof self!=='undefined'?self:globalThis);
