<template>
  <div>
    <input v-model="product.name" placeholder="Product Name" />
    <button @click="sendProduct">Send Product</button>
    <ul>
      <li v-for="message in messages" :key="message">{{ message }}</li>
    </ul>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import * as signalR from "@microsoft/signalr";

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
</script>

<style scoped>
/* Thêm các styles của bạn ở đây nếu cần */
</style>
