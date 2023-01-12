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

        const ftth = (url) => $.get(url,
            {}, function (resposta) {
                $('.container-filter').children().slice(1, 4).remove();
                $('.container-filter').append($(resposta).filter('#boxUpdate').html());
                $('#ftthLista').html($(resposta).filter('#tableUpdate').html());
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

        estacao = "";
        cdo = "";

        $('#dropEstacao').on('change', function () {
            spinnerFtth();
            estacao = 'estacao=' + this.value + '&';
            ftth('Construtora/lista?' + estacao);
            return false;
        });

        $('.container-filter, #dropCdo').on('change', function () {
            spinnerFtth();
            cdo = 'cdo=' + this.value + '&';
            ftth('Construtora/Lista?' + estacao + '' + cdo);
            return false;
        });

        $('#ftthLista, #paged').on('click', 'a', function () {
            spinnerFtth();

            let baseUrl = $(this).attr('href') + '&';

            ftth(baseUrl + '' + estacao + '' + cdo)
            return false;
        });
    })()
});

    