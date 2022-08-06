namespace ETLService.Services.Abstraction;

public interface IEtlService
{
    public Task OnAddNewFile(string path);

    public void MidnightReport();
}