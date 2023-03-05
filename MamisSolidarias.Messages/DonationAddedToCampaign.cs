namespace MamisSolidarias.Messages;

public enum Campaigns
{
    UnaMochiComoLaTuya,
    JuntosALaPar,
    Abrigaditos,
    ApadrinaMiSonriza,
    Navidemos,
    NavidadCompartida,
    Misiones
}

public enum Currency
{
    ARS,
    USD,
    EUR
}

/// <param name="DonationId">Id of the donation</param>
/// <param name="ParticipantId">Id of the participant of the campaign</param>
/// <param name="DonorId">Id of the donor</param>
/// <param name="CampaignId">Id of the campaign</param>
/// <param name="Campaign">Campaign of the participant</param>
/// <param name="Amount">Amount of donation</param>
/// <param name="Currency">Currency of the donation</param>
public sealed record DonationAddedToCampaign(
    Guid DonationId,
    int? DonorId,
    int? ParticipantId,
    int CampaignId,
    Campaigns Campaign,
    decimal Amount,
    Currency Currency
);