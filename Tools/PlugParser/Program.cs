// See https://aka.ms/new-console-template for more information

// 处理路径
using Coder.Ioc;
using Coder.Serivces.Attributes;
using Coder.Serivces.Dependency;
using Suyaa;
using System.ComponentModel;
using System.Reflection;
using Coder.Serivces.Helpers;
using Coder.Serivces;
using System;

if (!args.Any())
{
    Console.WriteLine("缺少路径");
    return;
}

// 读取dll地址
string file = sy.IO.GetFullPath(args[0].Replace(".\\", "~/"));
Console.WriteLine($"[Dll] {file} ...");

if (!File.Exists(file))
{
    Console.WriteLine($"文件'{file}'不存在");
    return;
}

// 加载程序集
Assembly.LoadFrom(file);

List<CoderActionDescriptor> coderActions = new List<CoderActionDescriptor>();

// 获取所有程序集
System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
foreach (System.Reflection.Assembly assembly in assemblies)
{
    try
    {
        Type[] types = assembly.GetTypes();
        foreach (Type type in types)
        {
            if (type.IsAbstract) continue;
            // 判断是否实现 ICoderService 接口
            if (type.HasInterface<ICoderService>())
            {
                var actions = type.GetActions();
                coderActions.AddRange(actions);
            }
        }
    }
    catch
    {
    }
}

// 输出
coderActions = coderActions.OrderBy(d => d.Name).ToList();
foreach (var action in coderActions)
{
    var descriptionContent = action.Name;
    var description = action.MetaDatas.Where(d => d is DescriptionAttribute).FirstOrDefault();
    if (description is not null) descriptionContent = ((DescriptionAttribute)description).Description;
    Console.WriteLine($"{action.Name} - {descriptionContent}");
}
