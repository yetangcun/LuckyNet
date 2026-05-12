using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.AspNetCore;
using MqttServerLib.Model;
using System.Security.Cryptography.X509Certificates;

namespace MqttServerLib
{
    /// <summary>
    /// MqttServerModule
    /// </summary>
    public static class MqttServerModule
    {
        /// <summary>
        /// 初始化加载
        /// </summary>
        public static void MqttServerModuleLoad(this WebApplicationBuilder bld, IConfiguration cfg)
        {
            bld.Services.AddMqttConnectionHandler();
            bld.Services.AddConnections();

            var mqttOption = cfg.GetSection("MqttServerOption").Get<MqttServerOption>();
            if (mqttOption != null)
            {
                // 添加托管 MQTT 服务器服务
                bld.Services.AddHostedMqttServer(serverBuilder =>
                {
                    if (mqttOption != null)
                    {
                        MqttServerExt._option = mqttOption;

                        // TCP 端口配置
                        serverBuilder.WithDefaultEndpoint();
                        serverBuilder.WithDefaultEndpointPort(mqttOption.Port);

                        // TCP SSL 配置
                        if (mqttOption.SSLPort > 0 && !string.IsNullOrWhiteSpace(mqttOption.CertificatePath) && File.Exists(mqttOption.CertificatePath))
                        {
                            var certificate = new X509Certificate2(mqttOption.CertificatePath, mqttOption.CertificatePassword);
                            serverBuilder.WithEncryptedEndpoint();
                            serverBuilder.WithEncryptedEndpointPort(mqttOption.SSLPort);
                            serverBuilder.WithEncryptionCertificate(certificate);
                        }

                        // 其他配置
                        serverBuilder.WithPersistentSessions(true);
                        serverBuilder.WithConnectionBacklog(102400);
                        serverBuilder.WithTcpKeepAliveTime(mqttOption.MqttKeepAliveTime * 1000);
                        serverBuilder.WithDefaultCommunicationTimeout(TimeSpan.FromSeconds(10));
                        serverBuilder.WithTcpKeepAliveInterval(mqttOption.MqttKeepAliveInterval * 1000);
                    }
                });

                bld.Services.AddSingleton(serviceProvider =>
                {
                    // 从已注册的 IHostedService 中获取 MqttServer 实例
                    var hostedServices = serviceProvider.GetServices<IHostedService>();
                    var mqttServer = hostedServices.OfType<MQTTnet.Server.MqttServer>().FirstOrDefault();

                    if (mqttServer == null)
                    {
                        throw new InvalidOperationException("MqttServer not found in hosted services");
                    }

                    MqttServerExt._mqttServer = mqttServer;

                    return mqttServer;
                });

                bld.WebHost.ConfigureKestrel(options =>
                {
                    // 配置 HTTPS 端口 (用于WSS和HTTP API)
                    if (mqttOption.WssPort > 0 && !string.IsNullOrEmpty(mqttOption.CertificatePath))
                    {
                        options.ListenAnyIP(mqttOption.WssPort, listenOptions => listenOptions.UseHttps(mqttOption.CertificatePath, mqttOption.CertificatePassword));
                    }

                    // 配置 HTTP 端口 (用于WS和HTTP API)
                    if (mqttOption.WsPort > 0)
                        options.ListenAnyIP(mqttOption.WsPort);

                    // === 核心：启动原生 MQTT (TCP) 端口 ===
                    options.ListenAnyIP(mqttOption.Port, listenOptions => listenOptions.UseMqtt());  // 告诉 Kestrel，这个端口使用 MQTT 协议);
                });
            }
        }

        /// <summary>
        /// 初始化设置
        /// </summary>
        public static void MqttServerModuleInit(this WebApplication app, IConfiguration cfg)
        {
            app.UseMqttServer(server =>
            {
                server.StartedAsync += MqttServerExt.OnStartedAsync;
                server.ClientConnectedAsync += MqttServerExt.OnClientConnected;
                server.ClientDisconnectedAsync += MqttServerExt.OnClientDisconnected;
                server.ValidatingConnectionAsync += MqttServerExt.OnValidatingConnection;
                server.ClientSubscribedTopicAsync += MqttServerExt.OnClientSubscribedTopic;
                server.ClientUnsubscribedTopicAsync += MqttServerExt.OnClientUnsubscribedTopic;
                server.ApplicationMessageNotConsumedAsync += MqttServerExt.OnApplicationMessageNotConsumed;
            });

            app.MapConnectionHandler<MqttConnectionHandler>("/mqtt", options =>
            {
                options.WebSockets.SubProtocolSelector = protocolList => protocolList.FirstOrDefault() ?? "mqtt";
            });
        }
    }
}
