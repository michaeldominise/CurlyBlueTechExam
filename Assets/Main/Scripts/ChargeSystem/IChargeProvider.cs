public interface IChargeProvider
{
    event System.Action<float> OnChargeChanged;
    event System.Action OnChargeStart;
    event System.Action OnChargeEnd;
}