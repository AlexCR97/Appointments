<script lang="ts">
	import { AppHeader, PageHeader, Sidebar } from '$lib/components';

	const sidebarWidth = '250px';
</script>

<div class="border" style="width: 100vw; height: 100vh">
	<div class="d-flex w-100 h-100">
		<aside class="border-end h-100 overflow-y-auto" style:min-width={sidebarWidth}>
			<Sidebar />
		</aside>

		<div class="w-100 h-100 d-flex flex-column">
			<AppHeader />

			<PageHeader />

			<main class="w-100 h-100 overflow-y-auto">
				<slot />
			</main>
		</div>
	</div>
</div>
