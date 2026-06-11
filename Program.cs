using System;
using System.Collections.Generic;
using System.Linq;

/*
 * TEMPLATE ESAME C# - NEGOZIO ONLINE
 */

public class Program
{
    public static void Main()
    {
        ApplicazioneNegozio applicazione = new ApplicazioneNegozio();

        // Per usare il menu vero:
        // applicazione.Avvia();

        // Per eseguire i test:
        TestNegozioOnline.EseguiTuttiITest();
    }
}

public class ApplicazioneNegozio
{
    private readonly CatalogoProdotti catalogoProdotti;
    private readonly CarrelloUtente carrelloUtente;
    private readonly StoricoAcquisti storicoAcquisti;
    private readonly ServizioNegozio servizioNegozio;

    public ApplicazioneNegozio()
    {
        catalogoProdotti = new CatalogoProdotti();
        carrelloUtente = new CarrelloUtente();
        storicoAcquisti = new StoricoAcquisti();
        servizioNegozio = new ServizioNegozio(catalogoProdotti, carrelloUtente, storicoAcquisti);

        CaricaDatiIniziali();
    }

    public void Avvia()
    {
        bool continua = true;

        while (continua)
        {
            Console.WriteLine("\n=== NEGOZIO ONLINE ===");
            Console.WriteLine("Scegli ruolo:");
            Console.WriteLine("1. Utente");
            Console.WriteLine("2. Amministratore");
            Console.WriteLine("0. Esci");

            string ruolo = ScegliRuolo();

            switch (ruolo)
            {
                case "utente":
                    GestisciMenuUtente();
                    break;

                case "amministratore":
                    GestisciMenuAmministratore();
                    break;

                case "esci":
                    continua = false;
                    Console.WriteLine("Uscita dal programma...");
                    break;

                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }
        }
    }

    private void CaricaDatiIniziali()
    {
        catalogoProdotti.AggiungiProdotto(new Prodotto("P001", "Tastiera meccanica", 79.90m, 10));
        catalogoProdotti.AggiungiProdotto(new Prodotto("P002", "Mouse wireless", 24.50m, 25));
        catalogoProdotti.AggiungiProdotto(new Prodotto("P003", "Monitor 24 pollici", 149.99m, 7));
        catalogoProdotti.AggiungiProdotto(new Prodotto("P004", "Cavo USB-C", 9.99m, 40));
    }

    private string ScegliRuolo()
    {
        Console.Write("Inserisci scelta: ");
        string? input = Console.ReadLine();

        input = input?.Trim().ToLower();

        return input switch
        {
            "1" => "utente",
            "utente" => "utente",
            "2" => "amministratore",
            "amministratore" => "amministratore",
            "admin" => "amministratore",
            "0" => "esci",
            "esci" => "esci",
            _ => ""
        };
    }

    private void GestisciMenuUtente()
    {
        bool continua = true;

        while (continua)
        {
            Console.WriteLine("\n=== MENU UTENTE ===");
            Console.WriteLine("1. Visualizza catalogo");
            Console.WriteLine("2. Aggiungi prodotto al carrello");
            Console.WriteLine("3. Visualizza carrello");
            Console.WriteLine("4. Modifica quantità nel carrello");
            Console.WriteLine("5. Rimuovi prodotto dal carrello");
            Console.WriteLine("6. Svuota carrello");
            Console.WriteLine("7. Conferma acquisto");
            Console.WriteLine("8. Visualizza storico acquisti utente");
            Console.WriteLine("0. Torna indietro");
            Console.Write("Scelta: ");

            string? scelta = Console.ReadLine();

            switch (scelta)
            {
                case "1":
                    MostraCatalogo();
                    break;

                case "2":
                    Console.Write("Codice prodotto: ");
                    string codiceAggiunta = Console.ReadLine() ?? "";
                    int quantitaAggiunta = LeggiInteroPositivo("Quantità: ");

                    if (servizioNegozio.AggiungiProdottoAlCarrello(codiceAggiunta, quantitaAggiunta))
                    {
                        Console.WriteLine("Prodotto aggiunto al carrello.");
                    }
                    else
                    {
                        Console.WriteLine("Impossibile aggiungere il prodotto.");
                    }
                    break;

                case "3":
                    MostraCarrello();
                    break;

                case "4":
                    Console.Write("Codice prodotto: ");
                    string codiceModifica = Console.ReadLine() ?? "";
                    int nuovaQuantita = LeggiInteroPositivo("Nuova quantità: ");

                    if (carrelloUtente.ModificaQuantitaNelCarrello(codiceModifica, nuovaQuantita))
                    {
                        Console.WriteLine("Quantità modificata.");
                    }
                    else
                    {
                        Console.WriteLine("Impossibile modificare la quantità.");
                    }
                    break;

                case "5":
                    Console.Write("Codice prodotto da rimuovere: ");
                    string codiceRimozione = Console.ReadLine() ?? "";

                    if (carrelloUtente.RimuoviDalCarrello(codiceRimozione))
                    {
                        Console.WriteLine("Prodotto rimosso dal carrello.");
                    }
                    else
                    {
                        Console.WriteLine("Prodotto non trovato nel carrello.");
                    }
                    break;

                case "6":
                    carrelloUtente.SvuotaCarrello();
                    Console.WriteLine("Carrello svuotato.");
                    break;

                case "7":
                    Console.Write("Nome utente: ");
                    string nomeUtente = Console.ReadLine() ?? "";

                    try
                    {
                        Acquisto acquisto = servizioNegozio.ConfermaAcquisto(nomeUtente);
                        Console.WriteLine("Acquisto confermato.");
                        servizioNegozio.StampaAcquisto(acquisto);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Errore: {ex.Message}");
                    }
                    break;

                case "8":
                    MostraStoricoUtente();
                    break;

                case "0":
                    continua = false;
                    break;

                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }
        }
    }

    private void GestisciMenuAmministratore()
    {
        bool continua = true;

        while (continua)
        {
            Console.WriteLine("\n=== MENU AMMINISTRATORE ===");
            Console.WriteLine("1. Visualizza catalogo");
            Console.WriteLine("2. Aggiungi prodotto");
            Console.WriteLine("3. Elimina prodotto");
            Console.WriteLine("4. Modifica prezzo");
            Console.WriteLine("5. Modifica quantità disponibile");
            Console.WriteLine("6. Visualizza tutti gli acquisti");
            Console.WriteLine("7. Visualizza report prodotti");
            Console.WriteLine("0. Torna indietro");
            Console.Write("Scelta: ");

            string? scelta = Console.ReadLine();

            switch (scelta)
            {
                case "1":
                    MostraCatalogo();
                    break;

                case "2":
                    Console.Write("Codice prodotto: ");
                    string codice = Console.ReadLine() ?? "";

                    Console.Write("Nome prodotto: ");
                    string nome = Console.ReadLine() ?? "";

                    decimal prezzo = LeggiPrezzoPositivo("Prezzo: ");
                    int quantita = LeggiInteroPositivo("Quantità iniziale: ");

                    try
                    {
                        catalogoProdotti.AggiungiProdotto(new Prodotto(codice, nome, prezzo, quantita));
                        Console.WriteLine("Prodotto aggiunto.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Errore: {ex.Message}");
                    }
                    break;

                case "3":
                    Console.Write("Codice prodotto da eliminare: ");
                    string codiceElimina = Console.ReadLine() ?? "";

                    if (catalogoProdotti.EliminaProdotto(codiceElimina))
                    {
                        Console.WriteLine("Prodotto eliminato.");
                    }
                    else
                    {
                        Console.WriteLine("Prodotto non trovato.");
                    }
                    break;

                case "4":
                    Console.Write("Codice prodotto: ");
                    string codicePrezzo = Console.ReadLine() ?? "";

                    decimal nuovoPrezzo = LeggiPrezzoPositivo("Nuovo prezzo: ");

                    try
                    {
                        if (catalogoProdotti.ModificaPrezzoProdotto(codicePrezzo, nuovoPrezzo))
                        {
                            Console.WriteLine("Prezzo modificato.");
                        }
                        else
                        {
                            Console.WriteLine("Prodotto non trovato.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Errore: {ex.Message}");
                    }
                    break;

                case "5":
                    Console.Write("Codice prodotto: ");
                    string codiceQuantita = Console.ReadLine() ?? "";

                    Console.Write("Variazione quantità, esempio 5 oppure -3: ");
                    bool valido = int.TryParse(Console.ReadLine(), out int variazione);

                    if (!valido)
                    {
                        Console.WriteLine("Valore non valido.");
                        break;
                    }

                    try
                    {
                        if (catalogoProdotti.ModificaQuantitaProdotto(codiceQuantita, variazione))
                        {
                            Console.WriteLine("Quantità modificata.");
                        }
                        else
                        {
                            Console.WriteLine("Prodotto non trovato.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Errore: {ex.Message}");
                    }
                    break;

                case "6":
                    List<Acquisto> acquisti = storicoAcquisti.OttieniTuttiGliAcquisti();

                    if (acquisti.Count == 0)
                    {
                        Console.WriteLine("Nessun acquisto registrato.");
                    }
                    else
                    {
                        foreach (Acquisto acquisto in acquisti)
                        {
                            servizioNegozio.StampaAcquisto(acquisto);
                        }
                    }
                    break;

                case "7":
                    servizioNegozio.StampaReportProdotti();
                    break;

                case "0":
                    continua = false;
                    break;

                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }
        }
    }

    private void MostraCatalogo()
    {
        List<Prodotto> prodotti = catalogoProdotti.OttieniTuttiIProdotti();

        Console.WriteLine("\n=== CATALOGO PRODOTTI ===");

        if (prodotti.Count == 0)
        {
            Console.WriteLine("Catalogo vuoto.");
            return;
        }

        foreach (Prodotto prodotto in prodotti)
        {
            Console.WriteLine($"{prodotto.CodiceProdotto} - {prodotto.Nome} - €{prodotto.Prezzo:F2} - Disponibili: {prodotto.QuantitaDisponibile}");
        }
    }

    private void MostraCarrello()
    {
        List<ElementoCarrello> elementi = carrelloUtente.OttieniElementi();

        Console.WriteLine("\n=== CARRELLO ===");

        if (elementi.Count == 0)
        {
            Console.WriteLine("Il carrello è vuoto.");
            return;
        }

        foreach (ElementoCarrello elemento in elementi)
        {
            Console.WriteLine($"{elemento.ProdottoSelezionato.CodiceProdotto} - {elemento.ProdottoSelezionato.Nome} - Quantità: {elemento.QuantitaScelta} - Prezzo: €{elemento.PrezzoUnitario:F2} - Totale: €{elemento.CalcolaTotaleParziale():F2}");
        }

        Console.WriteLine($"Totale carrello: €{carrelloUtente.CalcolaTotale():F2}");
    }

    private void MostraStoricoUtente()
    {
        Console.Write("Nome utente: ");
        string nomeUtente = Console.ReadLine() ?? "";

        List<Acquisto> acquisti = storicoAcquisti.OttieniAcquistiPerUtente(nomeUtente);

        if (acquisti.Count == 0)
        {
            Console.WriteLine("Nessun acquisto trovato per questo utente.");
            return;
        }

        foreach (Acquisto acquisto in acquisti)
        {
            servizioNegozio.StampaAcquisto(acquisto);
        }
    }

    private int LeggiInteroPositivo(string messaggio)
    {
        int valore;

        do
        {
            Console.Write(messaggio);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out valore) && valore > 0)
            {
                return valore;
            }

            Console.WriteLine("Inserisci un numero intero maggiore di zero.");

        } while (true);
    }

    private decimal LeggiPrezzoPositivo(string messaggio)
    {
        decimal valore;

        do
        {
            Console.Write(messaggio);
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, out valore) && valore > 0)
            {
                return valore;
            }

            Console.WriteLine("Inserisci un prezzo maggiore di zero.");

        } while (true);
    }
}

public interface IGestioneCatalogo
{
    void AggiungiProdotto(Prodotto prodotto);
    bool EliminaProdotto(string codiceProdotto);
    Prodotto? CercaProdottoPerCodice(string codiceProdotto);
    List<Prodotto> OttieniTuttiIProdotti();
    bool ModificaPrezzoProdotto(string codiceProdotto, decimal nuovoPrezzo);
    bool ModificaQuantitaProdotto(string codiceProdotto, int variazioneQuantita);
}

public interface IGestioneCarrello
{
    bool AggiungiAlCarrello(Prodotto prodotto, int quantita);
    bool ModificaQuantitaNelCarrello(string codiceProdotto, int nuovaQuantita);
    bool RimuoviDalCarrello(string codiceProdotto);
    void SvuotaCarrello();
    decimal CalcolaTotale();
    List<ElementoCarrello> OttieniElementi();
}

public interface IGestioneAcquisti
{
    void RegistraAcquisto(Acquisto acquisto);
    List<Acquisto> OttieniTuttiGliAcquisti();
    List<Acquisto> OttieniAcquistiPerUtente(string nomeUtente);
}

public class Prodotto
{
    public string CodiceProdotto { get; private set; }
    public string Nome { get; private set; }
    public decimal Prezzo { get; private set; }
    public int QuantitaDisponibile { get; private set; }
    public int QuantitaIniziale { get; private set; }

    public Prodotto(string codiceProdotto, string nome, decimal prezzo, int quantitaDisponibile)
    {
        CodiceProdotto = codiceProdotto;
        Nome = nome;
        Prezzo = prezzo;
        QuantitaDisponibile = quantitaDisponibile;
        QuantitaIniziale = quantitaDisponibile;
    }

    public void CambiaPrezzo(decimal nuovoPrezzo)
    {
        if (nuovoPrezzo <= 0)
        {
            throw new ArgumentException("Il prezzo deve essere maggiore di zero.");
        }

        Prezzo = nuovoPrezzo;
    }

    public void CambiaQuantita(int variazioneQuantita)
    {
        int nuovaQuantita = QuantitaDisponibile + variazioneQuantita;

        if (nuovaQuantita < 0)
        {
            throw new InvalidOperationException("La quantità disponibile non può diventare negativa.");
        }

        QuantitaDisponibile = nuovaQuantita;
    }

    public int CalcolaQuantitaVenduta()
    {
        return QuantitaIniziale - QuantitaDisponibile;
    }
}

public class ElementoCarrello
{
    public Prodotto ProdottoSelezionato { get; private set; }
    public int QuantitaScelta { get; private set; }
    public decimal PrezzoUnitario { get; private set; }

    public ElementoCarrello(Prodotto prodottoSelezionato, int quantitaScelta)
    {
        ProdottoSelezionato = prodottoSelezionato;
        QuantitaScelta = quantitaScelta;
        PrezzoUnitario = prodottoSelezionato.Prezzo;
    }

    public decimal CalcolaTotaleParziale()
    {
        return PrezzoUnitario * QuantitaScelta;
    }

    public void CambiaQuantitaScelta(int nuovaQuantita)
    {
        if (nuovaQuantita <= 0)
        {
            throw new ArgumentException("La quantità scelta deve essere maggiore di zero.");
        }

        QuantitaScelta = nuovaQuantita;
    }
}

public class Acquisto
{
    public string NomeUtente { get; private set; }
    public List<ElementoAcquistato> ProdottiAcquistati { get; private set; }
    public decimal TotaleOrdine { get; private set; }
    public DateTime DataAcquisto { get; private set; }

    public Acquisto(string nomeUtente, List<ElementoAcquistato> prodottiAcquistati)
    {
        NomeUtente = nomeUtente;
        ProdottiAcquistati = prodottiAcquistati;
        DataAcquisto = DateTime.Now;
        TotaleOrdine = CalcolaTotaleOrdine();
    }

    private decimal CalcolaTotaleOrdine()
    {
        return ProdottiAcquistati.Sum(prodotto => prodotto.TotaleParziale);
    }
}

public class ElementoAcquistato
{
    public string CodiceProdotto { get; private set; }
    public string NomeProdotto { get; private set; }
    public int QuantitaAcquistata { get; private set; }
    public decimal PrezzoUnitario { get; private set; }
    public decimal TotaleParziale { get; private set; }

    public ElementoAcquistato(string codiceProdotto, string nomeProdotto, int quantitaAcquistata, decimal prezzoUnitario)
    {
        CodiceProdotto = codiceProdotto;
        NomeProdotto = nomeProdotto;
        QuantitaAcquistata = quantitaAcquistata;
        PrezzoUnitario = prezzoUnitario;
        TotaleParziale = prezzoUnitario * quantitaAcquistata;
    }
}

public class CatalogoProdotti : IGestioneCatalogo
{
    private readonly List<Prodotto> prodotti;

    public CatalogoProdotti()
    {
        prodotti = new List<Prodotto>();
    }

    public void AggiungiProdotto(Prodotto prodotto)
    {
        bool codiceGiaPresente = prodotti.Any(p => p.CodiceProdotto == prodotto.CodiceProdotto);

        if (codiceGiaPresente)
        {
            throw new InvalidOperationException("Esiste già un prodotto con lo stesso codice.");
        }

        prodotti.Add(prodotto);
    }

    public bool EliminaProdotto(string codiceProdotto)
    {
        Prodotto? prodotto = CercaProdottoPerCodice(codiceProdotto);

        if (prodotto == null)
        {
            return false;
        }

        prodotti.Remove(prodotto);
        return true;
    }

    public Prodotto? CercaProdottoPerCodice(string codiceProdotto)
    {
        return prodotti.FirstOrDefault(prodotto =>
            prodotto.CodiceProdotto.Equals(codiceProdotto, StringComparison.OrdinalIgnoreCase));
    }

    public List<Prodotto> OttieniTuttiIProdotti()
    {
        return new List<Prodotto>(prodotti);
    }

    public bool ModificaPrezzoProdotto(string codiceProdotto, decimal nuovoPrezzo)
    {
        Prodotto? prodotto = CercaProdottoPerCodice(codiceProdotto);

        if (prodotto == null)
        {
            return false;
        }

        prodotto.CambiaPrezzo(nuovoPrezzo);
        return true;
    }

    public bool ModificaQuantitaProdotto(string codiceProdotto, int variazioneQuantita)
    {
        Prodotto? prodotto = CercaProdottoPerCodice(codiceProdotto);

        if (prodotto == null)
        {
            return false;
        }

        prodotto.CambiaQuantita(variazioneQuantita);
        return true;
    }
}

public class CarrelloUtente : IGestioneCarrello
{
    private readonly List<ElementoCarrello> elementiCarrello;

    public CarrelloUtente()
    {
        elementiCarrello = new List<ElementoCarrello>();
    }

    public bool AggiungiAlCarrello(Prodotto prodotto, int quantita)
    {
        if (quantita <= 0)
        {
            return false;
        }

        if (quantita > prodotto.QuantitaDisponibile)
        {
            return false;
        }

        ElementoCarrello? elementoEsistente = elementiCarrello.FirstOrDefault(elemento =>
            elemento.ProdottoSelezionato.CodiceProdotto.Equals(prodotto.CodiceProdotto, StringComparison.OrdinalIgnoreCase));

        if (elementoEsistente != null)
        {
            int nuovaQuantita = elementoEsistente.QuantitaScelta + quantita;

            if (nuovaQuantita > prodotto.QuantitaDisponibile)
            {
                return false;
            }

            elementoEsistente.CambiaQuantitaScelta(nuovaQuantita);
            return true;
        }

        elementiCarrello.Add(new ElementoCarrello(prodotto, quantita));
        return true;
    }

    public bool ModificaQuantitaNelCarrello(string codiceProdotto, int nuovaQuantita)
    {
        if (nuovaQuantita <= 0)
        {
            return false;
        }

        ElementoCarrello? elemento = elementiCarrello.FirstOrDefault(elemento =>
            elemento.ProdottoSelezionato.CodiceProdotto.Equals(codiceProdotto, StringComparison.OrdinalIgnoreCase));

        if (elemento == null)
        {
            return false;
        }

        if (nuovaQuantita > elemento.ProdottoSelezionato.QuantitaDisponibile)
        {
            return false;
        }

        elemento.CambiaQuantitaScelta(nuovaQuantita);
        return true;
    }

    public bool RimuoviDalCarrello(string codiceProdotto)
    {
        ElementoCarrello? elemento = elementiCarrello.FirstOrDefault(elemento =>
            elemento.ProdottoSelezionato.CodiceProdotto.Equals(codiceProdotto, StringComparison.OrdinalIgnoreCase));

        if (elemento == null)
        {
            return false;
        }

        elementiCarrello.Remove(elemento);
        return true;
    }

    public void SvuotaCarrello()
    {
        elementiCarrello.Clear();
    }

    public decimal CalcolaTotale()
    {
        return elementiCarrello.Sum(elemento => elemento.CalcolaTotaleParziale());
    }

    public List<ElementoCarrello> OttieniElementi()
    {
        return new List<ElementoCarrello>(elementiCarrello);
    }
}

public class StoricoAcquisti : IGestioneAcquisti
{
    private readonly List<Acquisto> acquisti;

    public StoricoAcquisti()
    {
        acquisti = new List<Acquisto>();
    }

    public void RegistraAcquisto(Acquisto acquisto)
    {
        acquisti.Add(acquisto);
    }

    public List<Acquisto> OttieniTuttiGliAcquisti()
    {
        return new List<Acquisto>(acquisti);
    }

    public List<Acquisto> OttieniAcquistiPerUtente(string nomeUtente)
    {
        return acquisti
            .Where(acquisto => acquisto.NomeUtente.Equals(nomeUtente, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

public class ServizioNegozio
{
    private readonly CatalogoProdotti catalogoProdotti;
    private readonly CarrelloUtente carrelloUtente;
    private readonly StoricoAcquisti storicoAcquisti;

    public ServizioNegozio(CatalogoProdotti catalogoProdotti, CarrelloUtente carrelloUtente, StoricoAcquisti storicoAcquisti)
    {
        this.catalogoProdotti = catalogoProdotti;
        this.carrelloUtente = carrelloUtente;
        this.storicoAcquisti = storicoAcquisti;
    }

    public bool AggiungiProdottoAlCarrello(string codiceProdotto, int quantita)
    {
        Prodotto? prodotto = catalogoProdotti.CercaProdottoPerCodice(codiceProdotto);

        if (prodotto == null)
        {
            return false;
        }

        return carrelloUtente.AggiungiAlCarrello(prodotto, quantita);
    }

    public Acquisto ConfermaAcquisto(string nomeUtente)
    {
        List<ElementoCarrello> elementi = carrelloUtente.OttieniElementi();

        if (elementi.Count == 0)
        {
            throw new InvalidOperationException("Non è possibile confermare un acquisto con carrello vuoto.");
        }

        foreach (ElementoCarrello elemento in elementi)
        {
            if (elemento.QuantitaScelta <= 0)
            {
                throw new InvalidOperationException("Nel carrello è presente una quantità non valida.");
            }

            if (elemento.QuantitaScelta > elemento.ProdottoSelezionato.QuantitaDisponibile)
            {
                throw new InvalidOperationException("La quantità richiesta supera la disponibilità di magazzino.");
            }
        }

        List<ElementoAcquistato> prodottiAcquistati = elementi
            .Select(elemento => new ElementoAcquistato(
                elemento.ProdottoSelezionato.CodiceProdotto,
                elemento.ProdottoSelezionato.Nome,
                elemento.QuantitaScelta,
                elemento.PrezzoUnitario))
            .ToList();

        foreach (ElementoCarrello elemento in elementi)
        {
            elemento.ProdottoSelezionato.CambiaQuantita(-elemento.QuantitaScelta);
        }

        Acquisto acquisto = new Acquisto(nomeUtente, prodottiAcquistati);
        storicoAcquisti.RegistraAcquisto(acquisto);
        carrelloUtente.SvuotaCarrello();

        return acquisto;
    }

    public List<ReportProdotto> CreaReportProdotti()
    {
        return catalogoProdotti.OttieniTuttiIProdotti()
            .Select(prodotto => new ReportProdotto(
                prodotto.CodiceProdotto,
                prodotto.Nome,
                prodotto.QuantitaIniziale,
                prodotto.CalcolaQuantitaVenduta(),
                prodotto.QuantitaDisponibile))
            .ToList();
    }

    public void StampaAcquisto(Acquisto acquisto)
    {
        Console.WriteLine("\n=== DETTAGLIO ACQUISTO ===");
        Console.WriteLine($"Utente: {acquisto.NomeUtente}");
        Console.WriteLine($"Data: {acquisto.DataAcquisto}");
        Console.WriteLine("Prodotti:");

        foreach (ElementoAcquistato prodotto in acquisto.ProdottiAcquistati)
        {
            Console.WriteLine($"{prodotto.CodiceProdotto} - {prodotto.NomeProdotto} - Quantità: {prodotto.QuantitaAcquistata} - Prezzo: €{prodotto.PrezzoUnitario:F2} - Totale: €{prodotto.TotaleParziale:F2}");
        }

        Console.WriteLine($"Totale ordine: €{acquisto.TotaleOrdine:F2}");
    }

    public void StampaReportProdotti()
    {
        List<ReportProdotto> report = CreaReportProdotti();

        Console.WriteLine("\n=== REPORT PRODOTTI ===");

        if (report.Count == 0)
        {
            Console.WriteLine("Nessun prodotto disponibile.");
            return;
        }

        foreach (ReportProdotto prodotto in report)
        {
            Console.WriteLine($"{prodotto.CodiceProdotto} - {prodotto.NomeProdotto} - Iniziale: {prodotto.QuantitaIniziale} - Venduta: {prodotto.QuantitaVenduta} - Disponibile: {prodotto.QuantitaDisponibile}");
        }
    }
}

public class ReportProdotto
{
    public string CodiceProdotto { get; private set; }
    public string NomeProdotto { get; private set; }
    public int QuantitaIniziale { get; private set; }
    public int QuantitaVenduta { get; private set; }
    public int QuantitaDisponibile { get; private set; }

    public ReportProdotto(string codiceProdotto, string nomeProdotto, int quantitaIniziale, int quantitaVenduta, int quantitaDisponibile)
    {
        CodiceProdotto = codiceProdotto;
        NomeProdotto = nomeProdotto;
        QuantitaIniziale = quantitaIniziale;
        QuantitaVenduta = quantitaVenduta;
        QuantitaDisponibile = quantitaDisponibile;
    }
}
