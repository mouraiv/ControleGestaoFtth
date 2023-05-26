//FUNÇÃO JS/JQUERY GLOBAL
$(document).ready(function () { 
    //VALIDAÇÃO DE FORMULÁRIO GLOBAL
    function validarForm() {
        'use strict'

        //PEGAR DOM DO FORMULARIO BOOTSTRAP PARA VALIDAÇÃO
        var forms = document.querySelectorAll('.needs-validation');

        //PERCORRER PROPS IMPUT EVENT
        Array.prototype.slice.call(forms)
            .forEach(function (form) {
                form.addEventListener('submit', function (event) {
                    if (!form.checkValidity()) {
                        event.preventDefault();
                        event.stopPropagation();
                    }

                    form.classList.add('was-validated');
                }, false);
            });

    } (validarForm());

    //CASO O BOTÃO NÃO SEJA ACIONADO, FECHAR A ALERT 10S
    /*setTimeout(function () {
       $('.alert').hide('hide');
    }, 10000);*/

    //BOTÃO FECHAR ALERTA
    $('.close-alert').click(function () {
        $('.alert').hide('hide');
    });

    //EFEITO DE ALTERNANCIA SETA DROPDOWN MENU
    $("#toggleBtn").click(function () {
        console.log($(".fa-caret-right"));
        $(".fa-caret-right").toggleClass("fa-caret-down");
    });

});
    