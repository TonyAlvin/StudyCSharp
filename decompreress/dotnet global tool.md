如果感觉自己写的程序可能到处都要用, 就可以把项目打包安装成一个global tool. 随后就可以在命令行中用命令直接调用写好的程序.

- 使用自己的项目建立DotNet Global Tool

## 修改``.csproj`` 文件
在csproj文件中的PropertyGroup节点内需要加入`<PackAsTool>true</PackAsTool>`. 如下
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <RootNamespace>global_tool</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
	<PackAsTool>true</PackAsTool>    // 我在这里
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="CommandDotNet" Version="7.0.1" />
    <PackageReference Include="spectre.console" Version="0.45.0" />
  </ItemGroup>
  
</Project>
```

## 打包
运行命令``dotnet pack``. 详见[[nuget#创建包]]. 

## 安装
执行global tool安装命令:``dotnet tool install [<PACKAGE_ID>] [options]`` 
常用用法 `` dotnet tool install -g <包名> --add-source <包所在的文件夹> ``
``install`` 的参数说明
```shell
Usage:    
  dotnet tool install [<PACKAGE_ID>] [options]

Arguments:
  <PACKAGE_ID>  要安装的工具的 NuGet 包 ID。

Options:
  -g, --global             为当前用户安装工具。
  --version <VERSION>      要安装的工具包版本。
  --tool-manifest <PATH>   清单文件的路径。
  --add-source <SOURCE>    添加其他要在安装期间使用的 NuGet 包源。
  --disable-parallel       防止并行还原多个项目。
  --ignore-failed-sources  将包源失败视为警告。
```
## 查看安装的global tool
`` dotnet tool list -g `` 查看安装global tool的包

## 卸载
`` dotnet tool uninstall -g <包的id> `` 卸载特定的包,可以先用`` list `` 查看包的id

