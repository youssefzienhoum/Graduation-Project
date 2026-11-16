using Chat.Client.Grpc.ServiceAbstraction;
using Chat.Client.Grpc.Services;
using MediaClient.Grpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClient.Grpc;

namespace Chat.Client.Grpc.DependancyInjection
{
    public static  class ServiceExtensions
    {
        public static IServiceCollection AddChatClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMediaStorageGrpcClient, MediaStorageGrpcClient>();
            services.AddScoped<IUserGrpcClient, UserGrpcClient>();
            services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(
                options =>
                {
                    options.Address = new Uri(
                    configuration["Grpc:UserServiceUrl"]!);
                });
          
            services.AddGrpcClient<MediaStorage.MediaStorageClient>(options =>
            {
                options.Address = new Uri(
                    configuration["Grpc:MediaStorageUrl"]!);
            });
            return services;
        }
    }
}
