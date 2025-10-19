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

    // Evento disparado quando a página aparece na tela.
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

    // Evento disparado quando o usuário clica no botão de adicionar Produto na Toolbar.
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

    // Evento disparado quando o usuário pesquisa algum Produto na caixa de busca.
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

    // Evento disparado quando o usuário realiza a Soma na Toolbar.
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O Total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");

    }

    // Evento disparado quando o usuário clica em um MenuItem da lista.
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Vai identificar qual MenuItem foi clicado a partir do 'sender'.
            MenuItem menuItem = (MenuItem)sender;

            // Então obtém o objeto 'Produto' associado àquela linha da lista.
            Produto produto_selecionado = (Produto)menuItem.BindingContext;

            // Pede confirmação ao usuário antes de apagar.
            bool confirmacao = await DisplayAlert("Atenção!", $"Tem certeza que deseja remover o produto '{produto_selecionado.Descricao}'?", "Sim, Remover", "Cancelar"
            );

            if (confirmacao)
            {
                // Remove o item do banco de dados.
                await App.Db.Delete(produto_selecionado.Id);

                // Remove o item da lista que está sendo exibida na tela.
                lista.Remove(produto_selecionado);

            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento disparado quando o usuário seleciona um item da lista.
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