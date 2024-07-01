<template>
  <div>
    <input v-model="product.name" placeholder="Product Name" />
    <button @click="sendProduct">Send Product</button>
    <ul>
      <!-- <li v-for="message in messages" :key="message">{{ message }}</li> -->
       {{ messages }}
    </ul>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import * as signalR from '@microsoft/signalr';

const product = ref({ name: '' });
const messages = ref("");

// const connection = new signalR.HubConnectionBuilder()
//   .withUrl("https://localhost:7020/notifications")
//   .build();

// connection.on("ReceiveMessage", (message) => {
//   messages.value = message;
// });

// onMounted(() => {
//   connection.start().catch(err => console.error(err.toString()));
// });

const sendProduct = async () => {
  await fetch('https://localhost:7020/api/product', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(product.value)
  }).then(response => response.json()).then(data => messages.value = data);
};
</script>

<style scoped>
/* Thêm các styles của bạn ở đây nếu cần */
</style>
