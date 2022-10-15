using ForwardAlgebraic.Effects.Abstractions;
using ForwardAlgebraic.Effects.Actor.Abstractions;
using Proto;
using Proto.Cluster;
using Proto.Cluster.Partition;
using Proto.Remote.GrpcNet;
using Proto.Cluster.Testing;
using AutoFixture.Xunit2;

namespace ForwardAlgebraic.Effects.Actor.Tests;

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
        using var eff = new EffectCluster(system.Root);

        var q = Cluster<RT>.RequestAff<bool>("1", "HelloGrain", "What!!");
        var r = await q.Run(new(eff, cts));

        Assert.True(r.ThrowIfFail());

        await cluster.ShutdownAsync();


    }
    public readonly record struct RT(in IEffectCluster EffectCluster,
                                     CancellationTokenSource CancellationTokenSource)
        : HasEffectCancel<RT>, HasEffectCluster<RT>;

}
