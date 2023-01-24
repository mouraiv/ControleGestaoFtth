$(document).ready(function () {
    (() => {
        var xValues = ["2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2022", "2023"];
        var yValues = [5500, 99958, 4433, 2454, 1500, 256, 45821, 2368, 23677, 7852];
        var barColors = ["green", "green", "green", "green", "green", "green", "green", "green", "green", "green",];

        new Chart("myChart", {
            type: "bar",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: barColors,
                    data: yValues
                }]
            },
            options: {
                legend: { display: false },
                responsive: true,
                maintainAspectRatio: false,
                title: {
                    display: true,
                    text: "Grafico Geral CDO"
                }
            }
        });
    })()

    //FUNÇÃO REPONSÁVEL PELAS ACÕES DA VIEW CONSTRUTORA
    function contrutora() {

        //VARIAVEIS GLOBAIS LOCAL
        var estacao = "";
        var cdo = "";
        var cabo = "";
        var celula = "";

        //FUNÇÃO DE ATUALIZAÇÃO DISPLAY PARTIALVIEW TABELA FTTH BASE
        const ftth = (url) => $.get(url,
            {}, function (resposta) {
                $('#ftthLista').html($(resposta).filter('#tableUpdate').html());
                return false;
            });

        //DISPLAY ATUALIZAÇÃO DIV ELEMENT PANEL MENU FILTRO
        const panel = (url) => $.get(url,
            {}, function (resposta) {
                $('#filterMenu').html($(resposta).filter('#panelUpdate').html());
                return false;
        });

        //SPINNER LOADER
        const spinnerFtth = (display) => {
            $(display).html(`
            <div class= "d-flex justify-content-center">
                <div class="spinner-border text-secondary m-5" role="status">
                    <span class="sr-only">Loading...</span>
                </div>
            </div>
        `   )
        }

        // ATUALIZAR DISPLAY PARTIAL VIEW 
        ftth('Construtora/lista');

        //FILTRO DROPDOWNLIST DA TABELA FTTH BASE
        $('#dropEstacao').on('change', function () {
            spinnerFtth('#ftthLista');
            estacao = $('#dropEstacao').val();
            panel('Construtora/lista?estacao=' + estacao);
            ftth('Construtora/lista?estacao=' + estacao);
            return false;
        });

        $('.container-filter').on('change', '#dropCdo', function () {
            spinnerFtth('#ftthLista');
            cdo = $('#dropCdo').val();

            /* DESABILITAR DROPDOWNLIST "CABO" "CELULA" E RESETAR VARIAVEIS 
            GLOBAIS DOS MESMOS, CASO UM VALOR SEJA SET EM CDO */
            if (cdo != "") {
                cabo = "";
                celula = "";
                $('#dropCabo option[value = ""]').prop('selected', 'selected');
                $('#dropCelula option[value = ""]').prop('selected', 'selected');
                $('#dropCabo').attr('disabled', true);
                $('#dropCelula').attr('disabled', true);
            } else {
                $('#dropCabo').attr('disabled', false);
                $('#dropCelula').attr('disabled', false);
            }
            ftth('Construtora/Lista?estacao=' + estacao + '&cdo=' + cdo + '&cabo=' + cabo + '&celula=' + celula);
            return false;
        });

        $('.container-filter').on('change', '#dropCabo', function () {
            spinnerFtth('#ftthLista');
            cabo = $('#dropCabo').val();
            ftth('Construtora/Lista?estacao=' + estacao + '&cdo=' + cdo + '&cabo=' + cabo + '&celula=' + celula);
            return false;
        });

        $('.container-filter').on('change', '#dropCelula', function () {
            spinnerFtth('#ftthLista');
            celula = $('#dropCelula').val();
            ftth('Construtora/Lista?estacao=' + estacao + '&cdo=' + cdo + '&cabo=' + cabo + '&celula=' + celula);
            return false;
        });

        //PAGINAÇÃO DA TABELA FTTH BASE
        $('#ftthLista, #paged').on('click', '.btnPaged', function () {
            spinnerFtth('#ftthLista');
            let urlFtth = $(this).find('a').attr('href');
            ftth(urlFtth + '&estacao=' + estacao + '&cdo=' + cdo + '&cabo=' + cabo + '&celula=' + celula);
            return false;
        });

        //PEGAR VALORES DA TABELA FTTH BASE PARA REQUISIÇÃO ATUALIZAR
        $('#ftthLista').on('click', '.tableFtth', function () {
            let carregarId = $(this).attr("data-id");

            $.get('Construtora/Detalhe?id=' + carregarId,
                {}, function (resposta) {
                    spinnerFtth('modal-body');
                    $('.modal-body').html($(resposta).find('#displayDetalheFtth').html());
                    return false;
            });

            $(this).find('#exampleModal').modal({
                keyboard: true,
                backdrop: "static",
                show: false,

            });
        });

    }(contrutora());
});

    