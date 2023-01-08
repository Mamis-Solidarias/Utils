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

/// <param name="DonationId">Id of the donation</param>
/// <param name="ParticipantId">Id of the participant of the campaign</param>
/// <param name="DonorId">Id of the donor</param>
/// <param name="CampaignId">Id of the campaign</param>
/// <param name="Campaign">Campaign of the participant</param>
public sealed record DonationAddedToCampaign(
    Guid DonationId,
    int? DonorId,
    int? ParticipantId,
    int CampaignId,
    Campaigns Campaign
);