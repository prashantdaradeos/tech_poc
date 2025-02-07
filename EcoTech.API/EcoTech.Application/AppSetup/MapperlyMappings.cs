



namespace EcoTech.Application.AppSetup;
public interface IMapper
{
     SignUpSpRequest Map(SignUpRequestDto source);
     AvailableUserSpRequest Map(AvailableUserRequestDto source);
     OtpSpRequest Map(VerifyOtpRequestDto source);
}


[Mapper]
public partial class MapperlyMappings: IMapper
{
    public partial SignUpSpRequest Map(SignUpRequestDto source);
    public partial AvailableUserSpRequest Map(AvailableUserRequestDto source);
    public partial OtpSpRequest Map(VerifyOtpRequestDto source);

}
