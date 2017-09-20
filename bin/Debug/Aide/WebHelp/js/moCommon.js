function getParams(a){var d=[],b;d.hash="";if(b=/#\w+/.exec(a))d.hash=b[0];d.href=a.match(/^[^#]+/)+"";var a=a.replace(/#\w+/,""),e;b=1;for(a=a.split(/#!/g);b<a.length,e=a[b];b++){var f=e.split(/=/);e=f[0];(f=f[1])?f.match(/%/)&&decodeURI&&(f=decodeURI(f)):e.match(/[\.]/)?(f=e,e="url"):e.match(/^\w+$/)?(f=e,e="id"):(e=e.replace(/[^\w]+/g,""),f=!0);d[e.toLowerCase()]=f}d.url&&(d.url+=d.hash);return d}var oldURL;
function onLive(a,d){function b(){if(!oldURL||oldURL!=window.location.href)oldURL=window.location.href,(a?a:checkParams)()}b();setInterval(b,d?d:20)}var hl_class="highlight";
function highlight(a,d){if(!a.nodeName.match(/script|style/i)){if(a.hasChildNodes)for(var b=0;b<a.childNodes.length;b++)highlight(a.childNodes[b],d);3==a.nodeType&&(a.nodeValue.match(d)&&a.parentNode.className!=hl_class)&&(b=a.ownerDocument.createElement("span"),b.innerHTML=a.nodeValue.replace(d,'<span class="'+hl_class+'">$1</span>'),a.parentNode.replaceChild(b,a))}}function printPage(){window.print()}
function checkParams(){var a,d=getParams(window.location.href);if(d.print)printPage(),d.close&&window.close();else if(a=d.highlight||d.hl)highlight(document.body,RegExp("("+a.replace(/\/b/g,"\\b").replace(/\//g,"/")+")","gi")),d.hash&&(window.location.href=d.href+d.hash)}
function breadcrumbs(a,d,b,e){for(var e=e?e:" - ",f="";0<=a;)for(var f=""==f?e+'<span class="current">'+d.ts[a]+"</span>":e+'<a href="#!i='+a+'">'+d.ts[a]+"</a>"+f,h=d.tl[a];d.tl[a]>=h&&0<=a;)a--;document.getElementById(b?b:"Breadcrumbs").innerHTML=f}
function getCookie(a){var d=document.cookie.indexOf(a+"="),b=d+a.length+1;if(!d&&a!=document.cookie.substring(0,a.length)||-1==d)return"";a=document.cookie.indexOf(";",b);-1==a&&(a=document.cookie.length);return unescape(document.cookie.substring(b,a))}function setCookie(a,d,b,e,f,h){var g=new Date;g.setTime(g.getTime());b=b?864E5*b:31536E6;g=new Date(g.getTime()+b);document.cookie=a+"="+escape(d)+(b?";expires="+g.toGMTString():"")+(e?";path="+e:"")+(f?";domain="+f:"")+(h?";secure":"")}
function Bookmarks(a){this.name=a;this.all=function(){return getCookie(this.name)};this.toArray=function(){return this.all().match(/\w+/g)};this.check=function(a){return this.all().match(RegExp("\\b"+a+"\\b","i"))};this.toggle=function(a){this.check(a)?setCookie(this.name,this.all().replace(RegExp("\\b"+a+"\\b[^\\w]?","i"),"")):setCookie(this.name,this.all()+a+"/")}}
String.prototype.format||(String.prototype.format=function(){for(var a=this,d=0;d<arguments.length;d++)a=a.replace(RegExp("\\{"+d+"\\}","gi"),arguments[d]);return a});String.prototype.toInt||(String.prototype.toInt=function(){var a=parseInt(this);return isNaN(a)?0:a});Array.prototype.indexOf||(Array.prototype.indexOf=function(a,d){var b=this.length,e=Number(d)||0,e=e<0?Math.ceil(e):Math.floor(e);for(e<0&&(e=e+b);e<b;e++)if(e in this&&this[e]===a)return e;return-1});var indexData;
function IndexData(a,d){if(indexData)return indexData;indexData={ns:[],ps:[],ts:[],tl:[]};for(var b=null,e=null,f=0,h=1,g=0;g<data.ti.length;g++){var j=data.ti[g],i=data.ts[j],j=data.ps[j];c=i.charAt(0).toUpperCase();nextTitle=null;if(a){nextTitle=g<data.ti.length-1?data.ts[data.ti[g+1]]:null;if(e&&e!=i){e=null;h--}}if(d&&c!=b){b&&h--;b=c;indexData.ps[f]="#";indexData.ts[f]=c;indexData.tl[f]=h;h++;f++}if(a){if(!e&&i==nextTitle){e=i;indexData.ps[f]="#";indexData.ts[f]=e;indexData.tl[f]=h;h++;f++}e&&
(i=data.ts[data.ir[g]])}indexData.ps[f]=j;indexData.ts[f]=i;indexData.tl[f]=h;f++}return indexData}
function fixIE6Position(a){for(var a=a.split(","),d=0;d<a.length;d++){var b=document.getElementById(a[d]);if(b){var e=b.parentElement;if(b.isCalcHeight||(""+b.currentStyle.height).toInt()==0){b.style.height=(e.tagName=="BODY"?document.documentElement.clientHeight:e.clientHeight)-(""+b.currentStyle.top).toInt()-(""+b.currentStyle.bottom).toInt()-(""+b.currentStyle.paddingTop).toInt()-(""+b.currentStyle.paddingBottom).toInt();b.isCalcHeight=true}if(b.isCalcWidth||(""+b.currentStyle.width).toInt()==
0){b.style.width=(e.tagName=="BODY"?document.documentElement.clientWidth:e.clientWidth)-(""+b.currentStyle.left).toInt()-(""+b.currentStyle.right).toInt()-(""+b.currentStyle.paddingLeft).toInt()-(""+b.currentStyle.paddingRight).toInt();b.isCalcWidth=true}}}};
