mergeInto(LibraryManager.library, {

    function ObtemIdUsuarioNavegador(){

        const id_usuario = localStorage.getItem("id_usuario");
        if( id_usuario == null ){
                localStorage.setItem("id_usuario", 1);
        }

        return id_usuario;
    
    }

});