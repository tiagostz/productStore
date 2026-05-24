import { useEffect, useState } from "react";
import api from "./api/productsApi";
import "./App.css";

const initialForm = {
  name: "",
  sku: "",
  category: "",
  price: "",
  stockQuantity: "",
};

function App() {
  const [products, setProducts] = useState([]);
  const [form, setForm] = useState(initialForm);
  const [editingId, setEditingId] = useState(null);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(1);

  async function loadProducts() {
    const response = await api.get(
      `/products?page=${page}&pageSize=${pageSize}`
    );

    setProducts(response.data.items);
    setTotalPages(response.data.totalPages);
  }

  useEffect(() => {
    loadProducts();
  }, [page]);

  function handleChange(event) {
    const { name, value } = event.target;

    setForm({
      ...form,
      [name]: value,
    });
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setError("");
    setSuccess("");

    const payload = {
      name: form.name,
      sku: form.sku,
      category: form.category,
      price: Number(form.price),
      stockQuantity: Number(form.stockQuantity),
    };

    try {
      if (editingId) {
        await api.put(`/products/${editingId}`, {
          name: payload.name,
          category: payload.category,
          price: payload.price,
          stockQuantity: payload.stockQuantity,
        });

        setSuccess("Produto atualizado com sucesso.");
      } else {
        await api.post("/products", payload);
        setSuccess("Produto cadastrado com sucesso.");
      }

      setForm(initialForm);
      setEditingId(null);
      loadProducts();
    } catch (err) {
      setError(err.response?.data?.message || "Erro ao salvar produto.");
    }
  }

  function handleEdit(product) {
    setEditingId(product.id);
    setForm({
      name: product.name,
      sku: product.sku,
      category: product.category,
      price: product.price,
      stockQuantity: product.stockQuantity,
    });
    setError("");
    setSuccess("");
  }

  async function handleDelete(id) {
    const confirmDelete = window.confirm("Deseja remover este produto?");

    if (!confirmDelete) return;

    try {
      await api.delete(`/products/${id}`);
      setSuccess("Produto removido com sucesso.");
      setError("");
      loadProducts();
    } catch {
      setError("Erro ao remover produto.");
    }
  }

  function handleCancelEdit() {
    setEditingId(null);
    setForm(initialForm);
    setError("");
    setSuccess("");
  }

  return (
    <div className="container">
      <div className="header">
        <h1>Product Store</h1>
        <p>Gerenciamento de produtos</p>
      </div>

      <div className="card">
        <h2>{editingId ? "Editar Produto" : "Adicionar Produto"}</h2>

        {error && <div className="alert-error">{error}</div>}
        {success && <div className="alert-success">{success}</div>}

        <form onSubmit={handleSubmit} className="form-grid">
          <input
            name="name"
            placeholder="Nome"
            value={form.name}
            onChange={handleChange}
            required
          />

          <input
            name="sku"
            placeholder="SKU"
            value={form.sku}
            onChange={handleChange}
            required
            disabled={!!editingId}
          />

          <select
            name="category"
            value={form.category}
            onChange={handleChange}
            required

          >
            <option
              value=""
              className="placeholder-option"
            >
              Selecione uma categoria
            </option>

            <option value="Eletronicos">Eletronicos</option>
            <option value="Games">Games</option>
            <option value="Audio">Audio</option>
            <option value="Video">Video</option>
            <option value="Perifericos">Perifericos</option>
            <option value="Acessorios">Acessorios</option>
            <option value="Tecnologia">Cadeiras Gamer</option>
            <option value="Tecnologia">Papelaria</option>

            
          </select>

          <input
            name="price"
            placeholder="Preço"
            type="number"
            value={form.price}
            onChange={handleChange}
            required
          />

          <input
            name="stockQuantity"
            placeholder="Estoque"
            type="number"
            value={form.stockQuantity}
            onChange={handleChange}
            required
          />

          <button className="btn-primary" type="submit">
            {editingId ? "Salvar edição" : "Adicionar"}
          </button>

          {editingId && (
            <button
              className="btn-secondary"
              type="button"
              onClick={handleCancelEdit}
            >
              Cancelar
            </button>
          )}
        </form>
      </div>

      <div className="card">
        <h2>Produtos</h2>

        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome</th>
              <th>SKU</th>
              <th>Categoria</th>
              <th>Preço</th>
              <th>Estoque</th>
              <th>Ações</th>
            </tr>
          </thead>

          <tbody>
            {products.map((product) => (
              <tr key={product.id}>
                <td>{product.id}</td>
                <td>{product.name}</td>
                <td>{product.sku}</td>
                <td>{product.category}</td>
                <td>R$ {product.price}</td>
                <td>{product.stockQuantity}</td>
                <td>
                  <button
                    className="btn-edit"
                    onClick={() => handleEdit(product)}
                  >
                    Editar
                  </button>

                  <button
                    className="btn-delete"
                    onClick={() => handleDelete(product.id)}
                  >
                    Remover
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        <div className="pagination">
          <button
            onClick={() => setPage(page - 1)}
            disabled={page === 1}
          >
            Anterior
          </button>

          <span>Página {page}</span>

          <button
            onClick={() => setPage(page + 1)}
            disabled={page >= totalPages}
          >
            Próxima
          </button>
        </div>
      </div>
    </div>
  );
}

export default App;
