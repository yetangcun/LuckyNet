using MQTTnet.AspNetCore;
using MqttServerLib.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

namespace MqttServerLib
{
    /// <summary>
    /// MqttServerModule
    /// </summary>
    public static class MqttServerModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bld"></param>
        /// <param name="cfg"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void MqttServerModuleLoad(this WebApplicationBuilder bld, IConfiguration cfg)
        {
            bld.Services.AddMqttConnectionHandler();
            bld.Services.AddConnections();

            var mqttOption = cfg.GetSection("MqttServerOption").Get<MqttServerOption>();
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

            // 注册 MqttServer 单例，供 MqttServerExt 注入
            // 注意：AddHostedMqttServer 注册的是 IHostedService，我们需要获取实际的 MqttServer 实例
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

            // Kestrel 配置（用于 WebSocket）
            if (mqttOption != null)
            {
                bld.WebHost.UseKestrel(o =>
                {
                    o.ListenAnyIP(mqttOption.Port, l => l.UseMqtt()); // mqtt

                    if (mqttOption.WsPort>0)
                        o.ListenAnyIP(mqttOption.WsPort);  // ws

                    if (mqttOption.WssPort > 0 && !string.IsNullOrWhiteSpace(mqttOption.CertificatePath) && File.Exists(mqttOption.CertificatePath))
                    {
                        var certificate = new X509Certificate2(mqttOption.CertificatePath, mqttOption.CertificatePassword);
                        o.ListenAnyIP(mqttOption.WssPort, l => l.UseHttps(certificate)); // wss
                    }
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="cfg"></param>
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
