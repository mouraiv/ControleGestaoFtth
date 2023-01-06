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

        $.get('/Construtora/Lista',
            {}, function (resposta) {
                $('#ftthLista').html(resposta)
            });


        $('#ftthLista, #paged').on('click', 'a', function () {
            console.log("Ok")
            $('#ftthLista').html(`
            <div class= "d-flex justify-content-center">
                <div class="spinner-border text-secondary m-5" role="status">
                    <span class="sr-only">Loading...</span>
                </div>
            </div>
        `   )

            let baseURL = $(this).attr('href');
            $.get(baseURL,
                {}, function (resposta) {
                    $('#ftthLista').html(resposta)
                });
            return false;
        });
    })()
});

    