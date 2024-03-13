import {dotnet} from './_framework/dotnet.js'

const { setModuleImports, getAssemblyExports, getConfig } = await dotnet.create();

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);
const svg = exports.PicturesqueTags.Program.GeneratePtag("Hello from JS", 256, 256);

console.log(svg);

document.getElementById("output").innerHTML = svg;
