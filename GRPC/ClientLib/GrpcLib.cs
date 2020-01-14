using Grpc.Core;
using Grpc.Net.Client;
using System;
using SzolgProg_vizsga;

namespace ClientLib
{
    public static class GrpcLib
    {
        private static ChannelBase channel;
        /// <summary>
        /// Ezért kell külön lib: https://github.com/grpc/grpc/issues/20402#issuecomment-537635172
        /// </summary>
        public static Book.BookClient BookServiceInit
        {
            get
            {
                Settings.Load();
                channel = GrpcChannel.ForAddress(address: $"{Settings.ChannelProtocol}://{Settings.ChannelHost}:{Settings.ChannelPort}");
                BookClient = new Book.BookClient(channel);
                return BookClient;
            }
        }

        public static Book.BookClient BookClient { get; private set; }
    }
}
