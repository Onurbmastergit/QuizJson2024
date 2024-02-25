mergeInto(LibraryManager.library, {
    ObtemIdUsuarioNavegador: function() {
        var id_usuario = localStorage.getItem("id_usuario");
        if (id_usuario === null) {
            id_usuario = 1;
            localStorage.setItem("id_usuario", id_usuario);
        }
        return id_usuario;
    }
});