﻿using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Caller
{
    [Command("progress")]
    public class ProgressCommand : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            ILongOpServiceProxy proxy = 
                channel.RealmProxy.Services.GetCalleeProxy<ILongOpServiceProxy>();

            Progress<int> progress =
                new Progress<int>(i => Console.WriteLine("Got progress " + i));

            int result = await proxy.LongOpAsync(10, progress).ConfigureAwait(false);

            Console.WriteLine("Got result " + result);
        }
    }

    public interface ILongOpServiceProxy
    {
        [WampProcedure("com.myapp.longop")]
        [WampProgressiveResultProcedure]
        Task<int> LongOpAsync(int n, IProgress<int> progress);
    }
}