function changeRole(userId,role,add_remove) {
    $.post("/Admin/UpdateRole", { userId: userId, role: role, add_remove: add_remove }, function (data) {
        if (!data.success) {
            alert(data.message);
        }
    });
}