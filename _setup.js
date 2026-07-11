var fs=require('fs'),path=require('path');  
var base='F:/C# projects/Test 1/ECommerce/postman';  
fs.mkdirSync(path.join(base,'environments'),{recursive:true});  
fs.mkdirSync(path.join(base,'collections','ECommerce API'),{recursive:true});  
console.log('dirs ok');  
