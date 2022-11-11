## What
类比Python的pip

这是一个.NET平台的包(package)管理工具.
简单的讲, 我们可以使用nuget把自己写的程序打包, 上传. 也可以下载到已经打包好的程序.

![[./img/Pasted image 20221109144559.png]]
## how 如何使用

这里我们使用CLI工具, 这种方式最接近底层, 界面工具可以理解为对CLI工具的封装

### 查找包
百度, nuget.org ...根据自己想要的功能查找对应的包

### 项目中的包

常用的命令
``` shell
	dotnet add package <package_name>    // 添加一个包
	dotnet add package <package_name> -v <version>  // 添加特定版本的包

	dotnet list package    // 列出当前项目所引用的包

	dotnet remove package <package_name>  // 从项目中移除一个包
```

如何使用一个包（以``Newtonsoft.Json``为例）
首先在项目目录下执行添加包的指令
```
dotnet add package Newtonsoft.Json
```
在项目中的某个代码文件内要用到包内的功能时,需要先using相应的包
```
	using Newtonsoft.Json;
```
随后包内的类, 定义的符号等都可以正常使用.

#### 查看项目中包含的包的另一种方法

项目中包含的包可以在项目的`` .csproj ``中查看,如下所示的项目文件中引用了`` Newtonsoft.Json ``包
```json
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

### 创建包

打包命令(打包xxx项目)
```shell
	donet pack [-o <保存包的路径>] xxx.csproj
```
注意打包时会忽略以 ``.`` 开头的文件. 例如 ``.git`` 文件夹

- 我想打包的项目需要依赖于另一个项目怎么办
如果是依赖于另一个可以独立运行的项目, 建一般是先把另一个项目打包,再设置包依赖. 如果另一个项目不能独立运行, 它就是为我这个项目服务的那可以打包在一起.

- 如何设置包
在打包之前可以对项目添加``.nuspec`` 文件, 这个文件是打包的清单. 以我们的解压缩的包为例
```xml
<?xml version="1.0" encoding="utf-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd">
  <metadata>
	<id>Decompress.Toolbox</id>          <!-- ID -->
	<version>1.0.0</version>             <!-- 版本 -->
    <authors>Decompress.Toolbox</authors><!-- 作者 -->
    <description>实时解压缩命令行工具</description><!-- 描述 -->
    <releaseNotes>2022年5月5日            <!-- 更新记录 -->
				- 新增对数据打dma包的功能
				2022年4月23日
				- 增加显示程序集版本号功能</releaseNotes>
    <packageTypes>
      <packageType name="DotnetTool" />  <!-- 包类型 -->
    </packageTypes>
  </metadata>
  <files>                                <!-- 包含的文件 -->
	  <file src="readme.txt" target="" /> 
	  <file src="icon.png" target="" /> 
  </files> 
</package>
```


### 发布包
将给定服务器 URL 的 API 密钥保存到`NuGet.Config`。
```shell
	nuget setApiKey <API KEY> -Source <nuget服务器>
```
API KEY一般由网站所有者提供。

发布：
```shell
	nuget push xxx.nupkg -Source <nuget服务器> -ApiKey <API KEY>
```

### 私人nuget服务器
有些时候我们写的程序过于先进, 不适合在互联网上传播. 但我们又要在内部网或多台电脑之间共享nuget包怎么办呢. 
直接用U盘或者其他途径共享`` .nupkg `` 文件的方法也不是不行, 但明显不太优雅. 我们想要有一台自己的nuget服务器! 
这里只介绍BaGet的使用方法, 详见[[BaGet]]