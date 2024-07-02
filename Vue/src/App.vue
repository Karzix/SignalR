<template>
  <div>
    <input v-model="product.name" placeholder="Product Name" />
    <button @click="sendProduct">Send Product</button>
    <ul>
      <li v-for="message in messages" :key="message">{{ message }}</li>
    </ul>
  </div>
  <div>
    <input type="file" multiple @change="handleFileUpload" />
    <button @click="uploadFiles">Upload Files</button>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import * as signalR from "@microsoft/signalr";
import axios from 'axios';

const product = ref({ name: "" });
const messages = ref([]);
const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7020/notifications")
      .build();


onMounted(async () => {
  try {
    
     var result = await connection.start();
     connection.on("ReceiveMessage", (message) => {
  messages.value.push(message);
});
  } catch (err) {
    console.log(err);
  }
});

const sendProduct = async () => {
  await fetch("https://localhost:7020/api/product", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(product.value),
  })
    .then((response) => response.json())
    .then((data) => console.log(data));
};

const files = ref([]);

const handleFileUpload = (event) => {
  files.value = event.target.files;
};

const uploadFiles = async () => {
  if (files.value.length === 0) {
    alert('No files selected.');
    return;
  }
  const formData = new FormData();
  for (let i = 0; i < files.value.length; i++) {
    formData.append('files', files.value[i]);
  }

  try {
    const response = await axios.post('https://localhost:7020/api/product/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    alert('Files uploaded successfully.');
  } catch (error) {
    console.error(error);
  }
};
</script>

<style scoped>

</style>
