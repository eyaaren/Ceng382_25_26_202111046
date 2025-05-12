document.getElementById('export').addEventListener('click', function () {
    const selectedColumns = new Set();
    
    console.log("Edit modal verileri:", id, name, count, description);


    // Başlığa tıklayarak seçilen kolonlar
    document.querySelectorAll('.column-header.selected').forEach(header => {
        selectedColumns.add(header.getAttribute('data-column'));
    });

    const socket = new WebSocket('wss://localhost:54495/');

socket.onopen = function() {
    console.log("WebSocket bağlantısı açıldı.");
};

socket.onmessage = function(event) {
    console.log("Mesaj alındı:", event.data);
};

socket.onerror = function(error) {
    console.error("WebSocket hatası:", error);
};

socket.onclose = function() {
    console.log("WebSocket bağlantısı kapandı.");
};

// WebSocket durumu izlemek için bir fonksiyon ekleyelim
setInterval(() => {
    if (socket.readyState === WebSocket.OPEN) {
        console.log("WebSocket bağlantısı açık.");
    } else if (socket.readyState === WebSocket.CLOSED) {
        console.log("WebSocket bağlantısı kapalı.");
    }
}, 1000);  // Bağlantı durumunu her saniye kontrol edelim


    document.querySelectorAll('.edit-button').forEach(button => {
        button.addEventListener('click', function () {
            const id = this.dataset.id;
            const name = this.dataset.name;
            const count = this.dataset.count;
            const description = this.dataset.description;
    
            // Kontrol için log ekleyelim
            console.log("Edit modal verileri:", id, name, count, description); // Kontrol için
            
            // Sayıyı log'layarak kontrol et
            console.log("Parsed Count:", parseInt(count));  // Bu değer doğru alınıyor mu?
    
            // Eğer count doğru alınmışsa parseInt'i kullan
            const parsedCount = isNaN(parseInt(count)) ? 0 : parseInt(count);
            console.log("Validated Count:", parsedCount); // Bu log ile son değeri kontrol edebilirsiniz
            
            document.getElementById('editId').value = id;
            document.getElementById('editName').value = name;
            document.getElementById('editCount').value = finalCount;
console.log("Güncellenmiş count değeri:", finalCount);
            document.getElementById('editDescription').value = description;
        });
    });
    
    
    // Checkbox ile seçilen kolonlar
    document.querySelectorAll('.column-checkbox:checked').forEach(checkbox => {
        selectedColumns.add(checkbox.value);
    });

    const columnsArray = Array.from(selectedColumns);
    console.log("Selected columns:", columnsArray);

    const filter = document.getElementById('filterInput')?.value || "";
    const pageNumber = parseInt(document.getElementById('currentPage')?.value || "1");

    fetch('/Index?handler=ExportJson', {
        method: 'POST',
        body: JSON.stringify({
            selectedColumns: columnsArray.length > 0 ? columnsArray : ["ClassName", "StudentCount", "Description"],
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
