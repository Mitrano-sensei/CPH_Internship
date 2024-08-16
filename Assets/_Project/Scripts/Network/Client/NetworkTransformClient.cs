using Unity.Netcode.Components;

public class NetworkTransformClient : NetworkTransform
{
    protected override bool OnIsServerAuthoritative() => false;
}
