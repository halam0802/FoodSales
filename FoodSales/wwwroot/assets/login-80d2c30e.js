import{a as k,g as E,l as $,c as I,p as P,r as m,n as U,_ as z,j as a,f as F,ae as R,z as W,a9 as M,a2 as S,af as T,ag as A,ah as _,v as w,ai as K,aa as b,A as D,I as G,a3 as H,a4 as C,aj as N,ad as O}from"./index-7088cbc6.js";import{u as V,c as q,S as J,M as Q,T as B,I as X}from"./Snackbar-ca6cc925.js";import{B as Y}from"./Button-f1679581.js";function Z(o){return E("MuiLoadingButton",o)}const nn=k("MuiLoadingButton",["root","loading","loadingIndicator","loadingIndicatorCenter","loadingIndicatorStart","loadingIndicatorEnd","endIconLoadingEnd","startIconLoadingStart"]),e=nn,on=["children","disabled","id","loading","loadingIndicator","loadingPosition","variant"],an=o=>{const{loading:n,loadingPosition:s,classes:r}=o,d={root:["root",n&&"loading"],startIcon:[n&&`startIconLoading${P(s)}`],endIcon:[n&&`endIconLoading${P(s)}`],loadingIndicator:["loadingIndicator",n&&`loadingIndicator${P(s)}`]},c=F(d,Z,r);return I({},r,c)},tn=o=>o!=="ownerState"&&o!=="theme"&&o!=="sx"&&o!=="as"&&o!=="classes",sn=$(Y,{shouldForwardProp:o=>tn(o)||o==="classes",name:"MuiLoadingButton",slot:"Root",overridesResolver:(o,n)=>[n.root,n.startIconLoadingStart&&{[`& .${e.startIconLoadingStart}`]:n.startIconLoadingStart},n.endIconLoadingEnd&&{[`& .${e.endIconLoadingEnd}`]:n.endIconLoadingEnd}]})(({ownerState:o,theme:n})=>I({[`& .${e.startIconLoadingStart}, & .${e.endIconLoadingEnd}`]:{transition:n.transitions.create(["opacity"],{duration:n.transitions.duration.short}),opacity:0}},o.loadingPosition==="center"&&{transition:n.transitions.create(["background-color","box-shadow","border-color"],{duration:n.transitions.duration.short}),[`&.${e.loading}`]:{color:"transparent"}},o.loadingPosition==="start"&&o.fullWidth&&{[`& .${e.startIconLoadingStart}, & .${e.endIconLoadingEnd}`]:{transition:n.transitions.create(["opacity"],{duration:n.transitions.duration.short}),opacity:0,marginRight:-8}},o.loadingPosition==="end"&&o.fullWidth&&{[`& .${e.startIconLoadingStart}, & .${e.endIconLoadingEnd}`]:{transition:n.transitions.create(["opacity"],{duration:n.transitions.duration.short}),opacity:0,marginLeft:-8}})),en=$("span",{name:"MuiLoadingButton",slot:"LoadingIndicator",overridesResolver:(o,n)=>{const{ownerState:s}=o;return[n.loadingIndicator,n[`loadingIndicator${P(s.loadingPosition)}`]]}})(({theme:o,ownerState:n})=>I({position:"absolute",visibility:"visible",display:"flex"},n.loadingPosition==="start"&&(n.variant==="outlined"||n.variant==="contained")&&{left:n.size==="small"?10:14},n.loadingPosition==="start"&&n.variant==="text"&&{left:6},n.loadingPosition==="center"&&{left:"50%",transform:"translate(-50%)",color:(o.vars||o).palette.action.disabled},n.loadingPosition==="end"&&(n.variant==="outlined"||n.variant==="contained")&&{right:n.size==="small"?10:14},n.loadingPosition==="end"&&n.variant==="text"&&{right:6},n.loadingPosition==="start"&&n.fullWidth&&{position:"relative",left:-10},n.loadingPosition==="end"&&n.fullWidth&&{position:"relative",right:-10})),rn=m.forwardRef(function(n,s){const r=U({props:n,name:"MuiLoadingButton"}),{children:d,disabled:c=!1,id:h,loading:p=!1,loadingIndicator:g,loadingPosition:j="center",variant:i="text"}=r,u=z(r,on),x=V(h),f=g??a.jsx(R,{"aria-labelledby":x,color:"inherit",size:16}),l=I({},r,{disabled:c,loading:p,loadingIndicator:f,loadingPosition:j,variant:i}),L=an(l),t=p?a.jsx(en,{className:L.loadingIndicator,ownerState:l,children:f}):null;return a.jsxs(sn,I({disabled:c||p,id:x,ref:s},u,{variant:i,classes:L,ownerState:l,children:[l.loadingPosition==="end"?d:t,l.loadingPosition==="end"?t:d]}))}),dn=rn;function ln(){const o=W(),n=M(),s=S(),{login:r}=T(),[d,c]=m.useState(!1),[h,p]=m.useState(""),[g,j]=m.useState(""),[i,u]=m.useState({open:!1,vertical:"bottom",horizontal:"right",message:"",variant:"error"}),x=()=>{if(h===""||g===""){u({...i,open:!0,message:"Username and/or password must not be empty.",variant:"error"});return}s(C(!0)),r({username:h,password:g}).then(t=>{var v,y;if(s(C(!1)),!t||!((v=t==null?void 0:t.data)!=null&&v.status)){u({...i,open:!0,message:"Username and/or password incorrect.",variant:"error"});return}u({...i,open:!0,message:"Login successfully.",variant:"success"}),s(N((y=t==null?void 0:t.data)==null?void 0:y.data)),setTimeout(()=>n.push("/"),500)})},f=t=>{t.key==="Enter"&&x()},l=(t,v)=>{v!=="clickaway"&&u({...i,open:!1})},L=a.jsxs(a.Fragment,{children:[a.jsxs(b,{spacing:3,children:[a.jsx(B,{name:"username",label:"Username",value:h,onChange:t=>p(t.target.value),onKeyUp:f}),a.jsx(B,{name:"password",label:"Password",type:d?"text":"password",value:g,onChange:t=>j(t.target.value),onKeyUp:f,InputProps:{endAdornment:a.jsx(X,{position:"end",children:a.jsx(G,{onClick:()=>c(!d),edge:"end",children:a.jsx(H,{icon:d?"eva:eye-fill":"eva:eye-off-fill"})})})}})]}),a.jsx(dn,{fullWidth:!0,size:"large",type:"submit",variant:"contained",color:"inherit",onClick:x,sx:{my:3},children:"Login"})]});return a.jsxs(A,{sx:{..._({color:w(o.palette.background.default,.9),imgUrl:"/assets/background/overlay_4.jpg"}),height:1},children:[a.jsx(K,{sx:{position:"fixed",top:{xs:16,md:24},left:{xs:16,md:24}}}),a.jsx(b,{alignItems:"center",justifyContent:"center",sx:{height:1},children:a.jsxs(q,{sx:{p:5,width:1,maxWidth:420},children:[a.jsx(D,{variant:"h4",sx:{mb:3},children:"Sign in to Food Sales"}),L]})}),a.jsx(J,{open:i.open,autoHideDuration:3e3,message:i.message,onClose:l,anchorOrigin:{vertical:i.vertical,horizontal:i.horizontal},children:a.jsx(Q,{elevation:6,severity:i.variant,variant:"filled",onClose:l,sx:{width:"100%"},children:i.message})})]})}function hn(){return a.jsxs(a.Fragment,{children:[a.jsx(O,{children:a.jsx("title",{children:" Login | Food Sales "})}),a.jsx(ln,{})]})}export{hn as default};
