import { useState } from "react";

export default function ImageUploader() {
  const [selectedFile, setSelectedFile] = useState(null);
  const [processedImage, setProcessedImage] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleFileChange = (event) => {
    setSelectedFile(event.target.files[0]);
  };

  const handleUpload = async () => {
    if (!selectedFile) {
      alert("Por favor, selecione um arquivo primeiro.");
      return;
    }

    setLoading(true);
    const formData = new FormData();
    formData.append("file", selectedFile);

    try {
      const response = await fetch("http://192.168.1.170/detect", {
        method: "POST",
        body: formData,
      });
      
      if (!response.ok) {
        throw new Error("Erro ao enviar imagem");
      }

      const data = await response.json();
      if (data.fileName) {
        fetchProcessedImage(data.fileName);
      } else {
        throw new Error("Resposta inválida da API");
      }
    } catch (error) {
      alert(error.message);
    } finally {
      setLoading(false);
    }
  };

  const fetchProcessedImage = async (fileName) => {
    try {
      const imageUrl = `http://192.168.1.170/${fileName}`;
      setProcessedImage(imageUrl);
    } catch (error) {
      alert("Erro ao obter imagem processada");
    }
  };

  return (
    <div className="flex flex-col items-center p-4 space-y-4">
      <h1 className="text-2xl font-bold">Upload de Imagem</h1>
      <input type="file" accept="image/*" onChange={handleFileChange} className="border p-2" />
      <button
        onClick={handleUpload}
        className="px-4 py-2 bg-blue-500 text-white rounded disabled:bg-gray-400"
        disabled={loading}
      >
        {loading ? "Enviando..." : "Enviar Imagem"}
      </button>

      {processedImage && (
        <div className="mt-4">
          <h2 className="text-xl font-semibold">Imagem Processada:</h2>
          <img src={processedImage} alt="Imagem processada" className="mt-4 border p-2" />
        </div>
      )}
    </div>
  );
}
