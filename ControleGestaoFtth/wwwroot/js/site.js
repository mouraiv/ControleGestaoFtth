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

    function contrutora() {

        var estacao = "";
        var cdo = "";
        var cabo = "";
        var celula = "";

        const ftth = (url) => $.get(url,
            {}, function (resposta) {
                $('#ftthLista').html($(resposta).filter('#tableUpdate').html());
                return false;
            });

        const panel = (url) => $.get(url,
            {}, function (resposta) {
                $('#filterMenu').html($(resposta).filter('#panelUpdate').html());
                return false;
        });

        const spinnerFtth = () => {
            $('#ftthLista').html(`
            <div class= "d-flex justify-content-center">
                <div class="spinner-border text-secondary m-5" role="status">
                    <span class="sr-only">Loading...</span>
                </div>
            </div>
        `   )
        }

        ftth('Construtora/lista');

        $('#dropEstacao').on('change', function () {
            spinnerFtth();
            estacao = $('#dropEstacao').val();
            panel('Construtora/lista?estacao=' + estacao);
            ftth('Construtora/lista?estacao=' + estacao);
            return false;
        });

        $('.container-filter').on('change', '#dropCdo', function () {
            spinnerFtth();
            cdo = $('#dropCdo').val();
            if (cdo != "") {
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
            spinnerFtth();
            cabo = $('#dropCabo').val();
            ftth('Construtora/Lista?estacao=' + estacao + '&cdo=' + cdo + '&cabo=' + cabo + '&celula=' + celula);
            return false;
        });

        $('.container-filter').on('change', '#dropCelula', function () {
            spinnerFtth();
            celula = $('#dropCelula').val();
            ftth('Construtora/Lista?estacao=' + estacao + '&cdo=' + cdo + '&cabo=' + cabo + '&celula=' + celula);
            return false;
        });
      
        $('#ftthLista, #paged').on('click', '.btnPaged', function () {
            spinnerFtth();
            let urlFtth = $(this).find('a').attr('href');
            ftth(urlFtth + '&estacao=' + estacao + '&cdo=' + cdo + '&cabo=' + cabo + '&celula=' + celula);
            return false;
        });

    }(contrutora());
});

    