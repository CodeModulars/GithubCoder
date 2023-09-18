using Coder.Ioc;
using Coder.Ioc.Dependency;
using Coder.Ioc.ServiceCollection;
using Coder.Script;
using Coder.Script.Actuators.CommandStatements;
using Coder.Serivces.Dependency;
using GithubCoder;
using Suyaa.Arguments;

Console.Title = sy.Assembly.FullName;

// 获取参数 
var eargs = new EArguments(args);
// 插件初始化
//Coder.Plugs.Basic.EnvironmentService.arguments = eargs;
// 校验必要参数
List<string> keys = new List<string>() {
    ArgumentFlags.Organization,
    ArgumentFlags.Repository,
    ArgumentFlags.Branch,
    ArgumentFlags.Target,
    ArgumentFlags.Project,
};
foreach (var key in keys)
{
    if (!eargs.ContainsKey(ArgumentFlags.Organization))
    {
        Console.WriteLine($"缺少'{ArgumentFlags.Organization}'参数");
        return;
    }
}

// 获取参数
var org = eargs[ArgumentFlags.Organization];
var repo = eargs[ArgumentFlags.Repository];
var branch = eargs[ArgumentFlags.Branch];
var target = eargs[ArgumentFlags.Target];
var project = eargs[ArgumentFlags.Project];

// 组织Url
var url = $"https://github.com/{org}/{repo}/archive/refs/heads/{branch}.zip";
var pathDown = sy.IO.GetFullPath("./down");
var pathOutput = sy.IO.GetFullPath("./output");
var file = sy.IO.CombinePath(pathDown, $"{org}-{repo}-{branch}.zip");
var unzipFolder = sy.IO.CombinePath(pathDown, $"{org}-{repo}-{branch}");
var outputFolder = sy.IO.CombinePath(pathOutput, $"{org}-{repo}-{branch}");

// 创建下载目录
sy.IO.CreateFolder(pathDown);
sy.IO.CreateFolder(outputFolder);

// 删除现有的文件
sy.IO.DeleteFile(file);
IOHelper.DeleteFolder(unzipFolder);

// 下载文件
Console.Write($"[Down] {url} ");
sy.Http.Download(url, file, opt =>
{
    opt.Cookies["name"] = $"{org}_{repo}_{branch}";
    opt.OnDownload(info =>
    {
        Console.Write('.');
    });
});
Console.WriteLine("Done");

// 解压
sy.IO.CreateFolder(unzipFolder);
Console.WriteLine($"[Unzip] {unzipFolder} ");
ZipHelper.UnZip(file, unzipFolder, entry =>
{
    Console.Write('.');
    return true;
});
Console.WriteLine("Done");

// 创建容器
IDependencyManager dm = new DependencyManager();
dm.RegisterAll();
// 注册参数
dm.Register(typeof(EArguments), eargs);


// 获取所有脚本文件
var scriptFiles = IOHelper.GetScriptFiles(unzipFolder);
foreach (var scriptFile in scriptFiles)
{
    // 获取相对路径
    var scriptFilePath = scriptFile.Substring(unzipFolder.Length);
    Console.Write($"[Run] {scriptFilePath} ... ");
    var scriptOutputPath = sy.IO.CombinePath(outputFolder, scriptFilePath);
    var scriptOutputFolder = System.IO.Path.GetDirectoryName(scriptOutputPath);
    sy.IO.CreateFolder(scriptOutputFolder!);
    // 读取脚本内容
    string content = sy.IO.ReadUtf8FileContent(scriptFile);
    // 加载脚本
    var engine = new Engine(dm, content);
    var types = dm.GetResolveTypes<ICommandStatementActuator>();
    // 输出中间脚本
    engine.Output(scriptOutputPath + ".s");
    // 执行脚本
    engine.Execute();
    Console.WriteLine("Done");
}
