using JetBrains.Annotations;

namespace ReservationSystem_Server.Helper.DataSeed;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
[MeansImplicitUse(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.WithMembers)]
[PublicAPI]
public class DataSeederAttribute : Attribute
{
}