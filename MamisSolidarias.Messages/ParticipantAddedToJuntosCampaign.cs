namespace MamisSolidarias.Messages;

public record ParticipantAddedToJuntosCampaign(
    int CampaignId,
    int BeneficiaryId
);