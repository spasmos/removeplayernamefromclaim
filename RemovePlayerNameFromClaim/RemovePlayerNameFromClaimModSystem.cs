using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Server;

namespace RemovePlayerNameFromClaim;

public class RemovePlayerNameFromClaimModSystem : ModSystem
{
    private const string HiddenOwnerName = "Unknown";

    private Harmony? harmony;

    public override void StartServerSide(ICoreServerAPI api)
    {
        harmony = new Harmony(Mod.Info.ModID);

        PatchPrefix(
            typeof(ServerWorldMap),
            "BroadcastClaims",
            nameof(BroadcastClaimsPrefix));

        PatchPrefix(
            typeof(ServerWorldMap),
            "SendClaims",
            nameof(SendClaimsPrefix));

        PatchPrefix(
            typeof(ServerMain),
            "SendIngameError",
            nameof(SendIngameErrorPrefix));
    }

    public override void Dispose()
    {
        harmony?.UnpatchAll(Mod.Info.ModID);
        harmony = null;
    }

    private void PatchPrefix(Type targetType, string targetMethodName, string prefixMethodName)
    {
        MethodInfo original = AccessTools.Method(targetType, targetMethodName);
        MethodInfo prefix = AccessTools.Method(typeof(RemovePlayerNameFromClaimModSystem), prefixMethodName);

        harmony?.Patch(original, prefix: new HarmonyMethod(prefix));
    }

    public static bool BroadcastClaimsPrefix(
        ServerWorldMap __instance,
        IEnumerable<LandClaim>? allClaims,
        IEnumerable<LandClaim>? addClaims)
    {
        ServerMain server = GetServer(__instance);
        server.BroadcastPacket(new Packet_Server
        {
            Id = 75,
            LandClaims = BuildSanitizedPacket(allClaims, addClaims)
        });

        return false;
    }

    public static bool SendClaimsPrefix(
        ServerWorldMap __instance,
        IServerPlayer player,
        IEnumerable<LandClaim>? allClaims,
        IEnumerable<LandClaim>? addClaims)
    {
        ServerMain server = GetServer(__instance);
        server.SendPacket(player, new Packet_Server
        {
            Id = 75,
            LandClaims = BuildSanitizedPacket(allClaims, addClaims)
        });

        return false;
    }

    public static void SendIngameErrorPrefix(string errorCode, ref object[] langparams)
    {
        if (!IsLandClaimPrivilegeError(errorCode) || langparams.Length == 0)
        {
            return;
        }

        langparams[0] = HiddenOwnerName;
    }

    private static bool IsLandClaimPrivilegeError(string errorCode)
    {
        return errorCode.Contains("landclaimed", StringComparison.OrdinalIgnoreCase);
    }

    private static ServerMain GetServer(ServerWorldMap worldMap)
    {
        FieldInfo serverField = AccessTools.Field(typeof(ServerWorldMap), "server")
            ?? throw new MissingFieldException(typeof(ServerWorldMap).FullName, "server");

        return (ServerMain)(serverField.GetValue(worldMap)
            ?? throw new InvalidOperationException("ServerWorldMap.server is null."));
    }

    private static Packet_LandClaims BuildSanitizedPacket(
        IEnumerable<LandClaim>? allClaims,
        IEnumerable<LandClaim>? addClaims)
    {
        Packet_LandClaims packet = new Packet_LandClaims();

        if (allClaims != null)
        {
            packet.SetAllclaims(allClaims.Select(ToSanitizedPacket).ToArray());
        }

        if (addClaims != null)
        {
            packet.SetAddclaims(addClaims.Select(ToSanitizedPacket).ToArray());
        }

        return packet;
    }

    private static Packet_LandClaim ToSanitizedPacket(LandClaim claim)
    {
        LandClaim sanitizedClaim = claim.Clone();
        sanitizedClaim.LastKnownOwnerName = HiddenOwnerName;

        Packet_LandClaim packet = new Packet_LandClaim();
        packet.SetData(SerializerUtil.Serialize(sanitizedClaim));
        return packet;
    }
}
