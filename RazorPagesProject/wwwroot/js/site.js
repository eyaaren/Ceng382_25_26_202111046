document.getElementById('export').addEventListener('click', function () {
    const selectedColumns = [];
    document.querySelectorAll('.column-header.selected').forEach(header => {
        selectedColumns.push(header.getAttribute('data-column'));
    });

    document.getElementById('export').addEventListener('click', function () {
        const selectedColumns = [];
        document.querySelectorAll('.column-checkbox:checked').forEach(checkbox => {
            selectedColumns.push(checkbox.value);
        });
    
        console.log("Selected columns:", selectedColumns); // Konsola log ekleyin
    
        const filter = document.getElementById('filterInput')?.value || "";
        const pageNumber = parseInt(document.getElementById('currentPage')?.value || "1");
    
        fetch('/Index?handler=ExportJson', {
            method: 'POST',
            body: JSON.stringify({
                selectedColumns: selectedColumns.length > 0 ? selectedColumns : ["ClassName", "StudentCount", "Description"], // Eğer hiçbiri seçilmezse tüm sütunları gönder
                filter,
                pageNumber
            }),
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
        // Diğer kodlar...
    });

    const filter = document.getElementById('filterInput')?.value || "";
    const pageNumber = parseInt(document.getElementById('currentPage')?.value || "1");

    fetch('/Index?handler=ExportJson', {
        method: 'POST',
        body: JSON.stringify({
            selectedColumns,
            filter,
            pageNumber
        }),
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        } 
    })
        .then(response => {
            if (!response.ok) throw new Error("Export işlemi başarısız.");
            return response.blob();
        })
        .then(blob => {
            const link = document.createElement('a');
            link.href = URL.createObjectURL(blob);
            link.download = 'exported_data.json';
            link.click();
        })
        .catch(error => alert(error.message));
});
