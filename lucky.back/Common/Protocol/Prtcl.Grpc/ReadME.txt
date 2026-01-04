﻿-- 首先是进到protoc.exe文件目录下
-- 通过protoc.exe把proto文件转换为.cs文件
powershell: .\protoc.exe --csharp_out=dst protos/gbasecore.proto --grpc_out=dst --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe
powershell: ./protoc.exe --csharp_out=dst protos/gtransbase.proto --grpc_out=dst --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe
powershell: ./protoc.exe --csharp_out=dst protos/gtrans.proto --grpc_out=dst --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe
powershell: ./protoc.exe --csharp_out=dst protos/gtrans.proto --grpc_out=dst --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe
cmd: protoc.exe --csharp_out=dst protos/gbasecore.proto --grpc_out=dst --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe



-- lx64目录是针对linux系统编译的工具