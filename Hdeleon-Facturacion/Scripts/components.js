//WindowLoad-----------------------------------------------------------------------------------------
function jsRemoveWindowLoad() {

    $("#WindowLoad").remove();

}

function jsShowWindowLoad( mensaje) {
    jsRemoveWindowLoad("", "");
   // if (rel === undefined) rel = "";
    if (mensaje === undefined) mensaje = "Realizando la operación...";
    //centrar imagen gif
    height = 20;//El div del titulo, para que se vea mas arriba (H)
    var ancho = 0;
    var alto = 0;
    if (window.innerWidth == undefined) ancho = window.screen.width;
    else ancho = window.innerWidth;
    if (window.innerHeight == undefined) alto = window.screen.height;
    else alto = window.innerHeight;

    var heightdivsito = alto / 2 - parseInt(height) / 2;//Se utiliza en el margen superior, para centrar la cosa

    imgCentro = "<div style='text-align:center;height:" + alto + "px;'><div  style='color:#000;margin-top:" + heightdivsito + "px; font-size:20px;font-weight:bold'>" + mensaje + "</div>" +
        "<img src='/Images/ajaxload.gif'></div>";

    //div grande------------------------------------------
    div = document.createElement("div");
    div.id = "WindowLoad"
    div.style.width = ancho + "px";
    div.style.height = alto + "px";
    $("body").append(div);

    //text pa quitar el foco
    input = document.createElement("input");
    input.id = "focusInput";
    input.type = "text"

    $("#WindowLoad").append(input);

    $("#focusInput").focus();
    $("#focusInput").hide();
    document.getElementById("WindowLoad").innerHTML = imgCentro;

}


//ventana de mensaje exito
function MessageSuccessful(mensaje, time) {

    var $ventana = $(window);

    ventanaAncho = $ventana.width();

    //si el ancho es mayor a 768 se hace esta funcionalidad
    if (ventanaAncho > 768) {

        if (time === undefined) time = 9000;

        //creamos el div
        $("body").append("<div class='Exito'></div>");

        $(".Exito").html(mensaje);
        $(".Exito")
	   .animate({ //seleccionamos el div

	       opacity: 1 //aparece el objeto
	   }, 1000  //la animación se realiza en 1 segundos
			   , function () { //ejecutamos un callback con función anonima para regresar a la normalidad el objeto
			       $(".Exito").animate({ //seleccionamos nuevamente el div
			           opacity: 0
			       }, time   //tiempo de 1 segundo
					, function () { $(this).remove(); }
				   )
			   })

    } else { //pantallas pequeñas un alert
        alert(mensaje);
    }
}

function MessageError(mensaje, time) {
    var $ventana = $(window);

    ventanaAncho = $ventana.width();

    //si el ancho es mayor a 768 se hace esta funcionalidad
    if (ventanaAncho > 768) {

        if (time === undefined) time = 6000;

        $("body").append("<div class='Error'></div>");

        $(".Error").html(mensaje);

        $(".Error")
      .animate({ //seleccionamos el div
          opacity: 1 //aparece
      }, 500  //la animación se realiza en 1 segundos
              , function () { //ejecutamos un callback con función anonima para regresar a la normalidad el objeto
                  $(".Error").animate({ //seleccionamos nuevamente el div
                      opacity: 0 //oculta

                  }, time   //tiempo de 1 segundo
                  , function () { $(this).remove(); }
                  )
              })

    } else { //pantallas pequeñas un alert
        alert(mensaje);
    }
}

function jsGetRandomName() {
    //obtención de carpeta
    myDate = new Date();
    diaActual = myDate.getDate() + (myDate.getMonth() + 1) + myDate.getFullYear() + myDate.getTime();
    nombre= "a" + diaActual + Math.floor((Math.random() * 100000) + 1);

    return nombre;

}
