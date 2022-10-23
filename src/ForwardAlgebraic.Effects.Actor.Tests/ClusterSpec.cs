using Algebraic.Effect.Abstractions;
using Algebraic.Effect.Actor;
using AutoFixture.Xunit2;
using Proto;
using Proto.Cluster;
using Proto.Cluster.Partition;
using Proto.Cluster.Testing;
using Proto.Remote.GrpcNet;

namespace Algebraic.Effect.Actor.Tests;

public class ClusterSpec
{
    [Theory, InlineAutoData]
    public async Task RequestAff(Guid clusterName)
    {
        var clusterKinds = new ClusterKind[]
        {
            new ClusterKind("HelloGrain", Props.FromFunc(ctx => ctx.Message switch
            {
                string m => Task.Run(() => ctx.Respond(true)),
                _ => Task.CompletedTask
            }))
        };

        var system = new ActorSystem().WithRemote(GrpcNetRemoteConfig.BindToLocalhost());

        var cluster = new Cluster(system,
            ClusterConfig.Setup(clusterName.ToString(), new TestProvider(new(), new()), new PartitionIdentityLookup())
                         .WithClusterKinds(clusterKinds));

        await cluster.StartMemberAsync();

        using var cts = new CancellationTokenSource();

        var q = Cluster<RT>.RequestAff<bool>("1", "HelloGrain", "What!!");
        var r = await q.Run(new(cluster, cts));

        Assert.True(r.ThrowIfFail());

        await cluster.ShutdownAsync();


    }
    public readonly record struct RT(in Cluster It,
                                     CancellationTokenSource CancellationTokenSource)
        : HasEffectCancel<RT>, Has<RT, Cluster>;

}
