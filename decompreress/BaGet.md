这是一个轻量化的NuGet服务器. 可以在自己的电脑上托管NuGet包.

## 运行BaGet
下载BaGet<https://github.com/loic-sharma/BaGet/releases>
在BaGet目录下运行命令:
```shell
	dotnet BaGet.dll
```

随后在浏览器内打开<http://localhost:5000/>即浏览或下载我们上传的包

## 创建/使用包
见[[nuget]]

## 发布包:
```shell
	dotnet nuget push -s http://localhost:5000/v3/index.json <包名>.nupkg
```
