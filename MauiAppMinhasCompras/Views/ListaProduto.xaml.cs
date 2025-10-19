using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

    // Evento disparado quando a p�gina aparece na tela.
    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

		    List<Produto> tmp = await App.Db.getAll();

		    tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    // Evento disparado quando o usu�rio clica no bot�o de adicionar Produto na Toolbar.
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "Ok");
		}
    }

    // Evento disparado quando o usu�rio pesquisa algum Produto na caixa de busca.
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
		catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }

    }

    // Evento disparado quando o usu�rio realiza a Soma na Toolbar.
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O Total � {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");

    }

    // Evento disparado quando o usu�rio clica em um MenuItem da lista.
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Vai identificar qual MenuItem foi clicado a partir do 'sender'.
            MenuItem menuItem = (MenuItem)sender;

            // Ent�o obt�m o objeto 'Produto' associado �quela linha da lista.
            Produto produto_selecionado = (Produto)menuItem.BindingContext;

            // Pede confirma��o ao usu�rio antes de apagar.
            bool confirmacao = await DisplayAlert("Aten��o!", $"Tem certeza que deseja remover o produto '{produto_selecionado.Descricao}'?", "Sim, Remover", "Cancelar"
            );

            if (confirmacao)
            {
                // Remove o item do banco de dados.
                await App.Db.Delete(produto_selecionado.Id);

                // Remove o item da lista que est� sendo exibida na tela.
                lista.Remove(produto_selecionado);

            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento disparado quando o usu�rio seleciona um item da lista.
    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto produto_selecionado = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = produto_selecionado,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }
}