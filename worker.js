importScripts('engine.js');
onmessage=e=>{const {dark,size,opts}=e.data;const g=Linify.run(dark,size,opts);let batch=[],r;
 while(!(r=g.next()).done){batch.push([r.value.from,r.value.to]);if(batch.length>=50){postMessage({type:'progress',step:r.value.step,lines:batch});batch=[];}}
 postMessage({type:'progress',step:-1,lines:batch});postMessage({type:'done'});};
