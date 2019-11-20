using ChinaBank.Api.Models;

public interface IGerencialRepository
{
    ContaPaginado ObterContas(ContaFiltro filtro);
    ExtratoPaginado ObterMovimentacoes(ExtratoFiltro filtro);
}