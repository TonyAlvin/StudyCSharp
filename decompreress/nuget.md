## What
类比Python的pip

这是一个.NET平台的包(package)管理工具.
简单的讲, 我们可以使用nuget把自己写的程序打包, 上传. 也可以下载到已经打包好的程序.

![[Pasted image 20221109144559.png]]
## how 如何使用
### 查找包
百度, nuget.org ...根据自己想要的功能查找对应的包

### 添加和管理包

常用的命令
```
	dotnet add package <package_name>    // 添加一个包
	dotnet add package <package_name> -v <version>  // 添加特定版本的包

	dotnet list package    // 列出当前项目所引用的包

	dotnet remove package <package_name>  // 从项目中移除一个包
```

### 项目中的包
项目中包含的包可以在项目的`` .csproj ``中查看,如下所示的项目文件中引用了`` Newtonsoft.Json ``包
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="13.0.1" />  // 我在这里
  </ItemGroup>
  
</Project>
```

在项目中要用到包内的功能时,需要先using相应的包
```
	using Newtonsoft.Json;
```
随后包内的类, 定义的符号等都可以正常使用.

### 创建包
### 发布包
### 私人nuget服务器
有些时候我们写的程序过于先进, 不适合在互联网上传播. 但我们又要在内部网或多台电脑之间共享nuget包怎么办呢. 
直接用U盘或者其他途径共享`` .nupkg `` 文件的方法也不是不行, 但明显不太优雅. 我们想要有一台自己的nuget服务器! 
这里只介绍 [[BaGet]] 的使用方法